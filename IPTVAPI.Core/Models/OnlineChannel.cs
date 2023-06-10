using System.Text.Json.Serialization;

namespace IPTVAPI.Core.Models;

public class OnlineChannel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } // string - Unique channel ID

    [JsonPropertyName("name")]
    public string Name { get; set; } // string - Full name of the channel

    [JsonPropertyName("country")]
    public string Country { get; set; } // string - Country code from which the broadcast is transmitted (ISO 3166-1 alpha-2)

    [JsonPropertyName("languages")]
    public List<string> Languages { get; set; } // array - List of languages broadcast

    [JsonPropertyName("categories")]
    public List<string> Categories { get; set; } // array - List of categories to which this channel belongs

    [JsonPropertyName("is_nsfw")]
    public bool IsNsfw { get; set; } // bool - Indicates whether the channel broadcasts adult content

    [JsonPropertyName("website")]
    public string Website { get; set; } // string - Official website URL

    [JsonPropertyName("logo")]
    public string Logo { get; set; } // string - Logo URL
}