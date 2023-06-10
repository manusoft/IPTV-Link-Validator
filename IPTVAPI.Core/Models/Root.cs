namespace IPTVAPI.Core.Models;

public class Root
{
    public int Total { get; set; }
    public string LastUpdated { get; set; }
    public List<RootChannel> Channels { get; set; }
}
