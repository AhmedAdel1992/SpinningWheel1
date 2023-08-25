using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wallet.Integration.Tests.Factories;

namespace Wallet.Integration.Tests.Endpoints.Base;

public class BaseEndpointTest
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;

    public BaseEndpointTest(ApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
        _options = new JsonSerializerOptions()
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
    
    protected async Task<HttpResponseMessage> PostAsync(string url, object command)
    {
        return await _client.PostAsJsonAsync(url, command,_options);
    }
    protected async Task<TResponse?> PostAsync<TResponse>(string url, object command)
    {
        var response= await _client.PostAsJsonAsync(url, command,_options);
        return await response.Content.ReadFromJsonAsync<TResponse>(_options);
    }
    
    protected async Task<HttpResponseMessage> GetAsync(string url,CancellationToken ct=default)
    {
        return await _client.GetAsync(url,ct);
    }
    protected async Task<TResponse?> GetAsync<TResponse>(string url,CancellationToken ct=default)
    {
        var response= await _client.GetAsync(url,ct);
        return await response.Content.ReadFromJsonAsync<TResponse>(_options, cancellationToken: ct);
    }
    
}