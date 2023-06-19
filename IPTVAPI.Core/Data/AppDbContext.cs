namespace IPTVAPI.Core.Data;

public class AppDbContext : DbContext
{
    public DbSet<OfflineChannel> Channels { get; set; }
    public DbSet<OfflineStream> Streams { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }
}
