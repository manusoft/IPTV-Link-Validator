namespace IPTVAPI.Core.Services;

public class DataService
{
    private readonly AppDbContext context;

    public DataService(AppDbContext _context)
    {
        this.context = _context;
    }

    public async Task CreateOrUpdateAsync(OfflineChannel channel, OfflineStream stream)
    {
        var channelFound = context.Channels.FirstOrDefault(c => c.Id == channel.Id);

        if (channelFound is null)
        {
            context.Channels.Add(channel);
        }

        var streamFound = context.Streams.FirstOrDefault(s => s.Url == stream.Url);

        if (streamFound is null)
        {
            context.Streams.Add(stream);
        }
        else
        {           
            streamFound.ChannelId = stream.ChannelId;
            streamFound.Url = stream.Url;
            streamFound.IsOnline = stream.IsOnline;
            streamFound.LastCheckedAt = stream.LastCheckedAt;
            context.Update(streamFound);
        }

        await context.SaveChangesAsync();
    }


    public async Task UpdateStreamAsync(OfflineStream entity)
    {
        var currentStream = context.Streams.FirstOrDefault(stream => stream.Id == entity.Id);

        if (currentStream is not null)
        {
            currentStream.CheckCount++;
            context.Streams.Update(currentStream);
            await context.SaveChangesAsync();
        }        
    }

    public async Task<IEnumerable<OfflineChannel>> GetChannelsAsync()
    {
        return await context.Channels
            .Include(c => c.Streams)
            .OrderBy(c => c.Id)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<OfflineStream>> GetStreamsAsync()
    {
        return await context.Streams
            .OrderBy(s => s.ChannelId)
            .AsNoTracking()
            .ToListAsync();
    }
}
