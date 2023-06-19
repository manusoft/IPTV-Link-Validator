namespace IPTVAPI.Core.Models;

[Table("Stream")]
public class OfflineStream
{
    public int Id { get; set; }
    public string ChannelId { get; set; } // Channel ID
    public string Url { get; set; } // Stream URL
    public bool IsOnline { get; set; } // Check for working link
    public int CheckCount { get; set; } // Inrcement counts each time of checking
    public DateTime LastCheckedAt { get; set; }
    public virtual OfflineChannel Channel { get; set; } = null!;
}

