using Microsoft.EntityFrameworkCore;

namespace NorthWind.EFCore.Repositories.DbContexts;
internal class QueryDbContext : DbContext
{
    public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<Entities.Category> Categories { get; set; }

    public DbSet<Entities.Product> Products { get; set; }

}
