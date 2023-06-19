namespace IPTVAPI.Core.Models;

public class OnlineStream
{
    [JsonPropertyName("channel")]
    public string Channel { get; set; } // Channel ID

    [JsonPropertyName("url")]
    public string Url { get; set; } // Stream URL
}

