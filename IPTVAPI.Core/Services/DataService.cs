namespace IPTVAPI.Core.Services;

public class DataService : IDisposable
{
    private readonly AppDbContext context;

    public DataService(AppDbContext _context)
    {
        this.context = _context;
    }

    public async Task CreateOrUpdateAsync(OfflineChannel channel, OfflineStream stream)
    {
        var channelFound = context.Channels
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == channel.Id);

        if (channelFound is null)
        {
            context.Channels.Add(channel);
        }

        var streamFound = context.Streams
            .AsNoTracking()
            .FirstOrDefault(s => s.Url == stream.Url);

        if (streamFound is null)
        {
            context.Streams.Add(stream);
        }
        else
        {
            streamFound.ChannelId = stream.ChannelId;
            streamFound.Url = stream.Url;
            streamFound.IsOnline = stream.IsOnline;
            streamFound.CheckCount = stream.CheckCount;
            streamFound.CreatedAt = stream.CreatedAt;
            streamFound.UpdatedAt = stream.UpdatedAt;
            context.Streams.Update(streamFound);
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public async Task UpdateStreamAsync(OfflineStream entity)
    {     
        try
        {
            //context.Streams.Update(entity);
            context.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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

    public void Dispose()
    {
        this.context.Dispose();
    }
}
