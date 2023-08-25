using RestSharp;

namespace Common.HttpClient.Base;

public interface IBaseRestClient
{
    Task<RestResponse> ExecuteWithLog(RestRequest request, CancellationToken stoppingToken);
}