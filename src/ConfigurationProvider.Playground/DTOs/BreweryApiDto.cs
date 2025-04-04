using System.Text.Json.Serialization;

namespace ConfigurationProvider.Playground.DTOs;

public class BreweryApiDto
{
    [JsonPropertyName("website_url")]
    public string? WebsiteUrl { get; set; }
}