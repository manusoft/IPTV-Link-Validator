using System.Text.Json.Serialization;

namespace IPTVAPI.Core.Models;

public class Country
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("languages")]
    public List<string>? Languages { get; set; }

    [JsonPropertyName("flag")]
    public string? Flag { get; set; }
}