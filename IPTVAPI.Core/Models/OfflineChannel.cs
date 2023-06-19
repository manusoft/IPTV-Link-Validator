namespace IPTVAPI.Core.Models;

[Table("Channel")]
public class OfflineChannel
{
    [Key]
    public string Id { get; set; } // string - Unique channel ID
    public string Name { get; set; } // string - Full name of the channel
    public string Country { get; set; } // string - Country code from which the broadcast is transmitted (ISO 3166-1 alpha-2)
    public string Languages { get; set; } // array - List of languages broadcast
    public string Categories { get; set; } // array - List of categories to which this channel belongs
    public bool IsNsfw { get; set; } // bool - Indicates whether the channel broadcasts adult content
    public string Website { get; set; } // string - Official website URL
    public string Logo { get; set; } // string - Logo URL

    public virtual ICollection<OfflineStream> Streams { get; set; } = new ObservableCollection<OfflineStream>();
}