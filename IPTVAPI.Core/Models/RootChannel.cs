using System.Collections.ObjectModel;

namespace IPTVAPI.Core.Models;

public class RootChannel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string Languages { get; set; }
    public string Categories { get; set; }
    public bool IsNsfw { get; set; }
    public string Website { get; set; }
    public string Logo { get; set; }
    public List<RootStream> Streams { get; set; }
}