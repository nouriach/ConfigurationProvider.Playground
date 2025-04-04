using System.Text.Json;
using ConfigurationProvider.Playground.DTOs;

namespace ConfigurationProvider.Playground.Configuration;

public class ApiConfigurationProvider : Microsoft.Extensions.Configuration.ConfigurationProvider
{
    private readonly ApiConfigurationSource _apiConfigurationSource;

    public ApiConfigurationProvider(ApiConfigurationSource apiConfigurationSource)
    {
        _apiConfigurationSource = apiConfigurationSource;
    } 

    public override void Load()
    {
        try
        {
            // ar httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{_apiConfigurationSource.RequestBaseUrl}?per_page=5");
            var url = $"{_apiConfigurationSource.RequestBaseUrl}?per_page=5";

            using var client = new HttpClient();
            Console.WriteLine($"---> Sending HTTP Request: {url}");
            var response = client.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
                CheckOptional();

            var responseContent = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var apiResponse = JsonSerializer.Deserialize<List<BreweryApiDto>>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });

            if (apiResponse is null || apiResponse.Count <= 0) 
                CheckOptional();

            var configData = new Dictionary<string, string?>();
                
            for (var i = 0; i < apiResponse.Count; i++)
            {
                Console.WriteLine($"---> Adding new Config Value. Key: AllowedOrigins__{i}, Value: {apiResponse[i].WebsiteUrl}");
                configData[$"AllowedOrigins__{i}"] = apiResponse[i].WebsiteUrl;
            }

            Data = configData;
            OnReload();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            CheckOptional();
        }
    }
    
    private void CheckOptional()
    {
        if (_apiConfigurationSource.Optional) return;

        Console.WriteLine("---> Failed to retrieve URLs. Config is not optional.");
        throw new Exception($"Cannot load config from {_apiConfigurationSource.RequestBaseUrl}");
    }
}