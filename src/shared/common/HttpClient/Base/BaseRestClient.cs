using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Logging;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Common.HttpClient.Base
{
    public abstract class BaseRestClient<T> : RestClient, IBaseRestClient
    {
        protected readonly ILoggerAdapter<T> _logger;
        protected int _timeout = -1;
        protected readonly JsonSerializerOptions _options;

        protected BaseRestClient(string baseUrl, ILoggerAdapter<T> logger):base(baseUrl)
        {
            _logger = logger;
            _options = new JsonSerializerOptions()
            {
                Converters = { new JsonStringEnumConverter() },
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        public async Task<RestResponse> ExecuteWithLog(RestRequest request, CancellationToken stoppingToken)
        {
            Log(request);
           // this.Timeout = _timeout;
            var response = await this.ExecuteAsync(request, stoppingToken);
            Log(response);
            return response;
        }

        public async Task<RestResponse<T>> ExecuteWithLog<T>(RestRequest request, CancellationToken stoppingToken)
        {
            Log(request);
            //this.Timeout = _timeout;
            var response = await this.ExecuteAsync<T>(request, stoppingToken);
            Log(response);
            return response;
        }


        private void Log(RestRequest request)
        {
            var authToken = GetParameter(request, "Authorization");
            var transactionId = GetParameter(request, "transaction-id");

            if (string.IsNullOrEmpty(transactionId))
            {
                _logger.LogInformation("Calling {Url} with content => {Content}", request.Resource,
                    ReadContent(request.Parameters.ToList()));
            }
            else
            {
                _logger.LogInformation("Calling {Url} TransId: {Uuid} ({AuthToken}) with content => {Content}",
                    request.Resource,
                    transactionId, authToken, ReadContent(request.Parameters.ToList()));
            }
        }

        private void Log(RestResponse response)
        {
            var request = response.Request;
            var responseString = response.Content;

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                _logger.LogInformation("Response for {Url} is ({Status}) with content => {Content}",
                    request.Resource, response.StatusCode, responseString);
                return;
            }

            _logger.LogError("Response failed => {Message}", response.ErrorMessage);

            var authToken = GetParameter(request, "Authorization");
            var transactionId = GetParameter(request, "transaction-id");

            if (string.IsNullOrEmpty(transactionId))
            {
                _logger.LogError("Response for {Url} is ({Status}) with content => {Content}",
                    request.Resource, response.StatusCode.ToString(), responseString);
            }
            else
            {
                _logger.LogError(
                    "Response for {Url} is ({Status}) TransId: {Uuid} ({AuthToken}) with content => {Content}",
                    request.Resource, response.StatusCode.ToString(), transactionId, authToken, responseString);
            }
        }

        private string GetParameter(RestRequest request, string paramName)
        {
            var value = request.Parameters.FirstOrDefault(a => a.Name == paramName);
            return value?.ToString();
        }

        private string ReadContent(List<Parameter> parameters)
        {
            var json = parameters.Find(z => z.Type == ParameterType.RequestBody);
            if (json == null)
            {
                var query = parameters.Find(z => z.Name == "application/x-www-form-urlencoded");
                return query != null ? (string)query.Value : "";
            }
            else
            {
                return json.Value.ToString();
            }
        }
    }
}