using Microsoft.EntityFrameworkCore;

namespace NorthWind.EFCore.Repositories.DbContexts;
internal class NorthWindSqlLiteDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=northwinddatabase.db");
    }

    public DbSet<Entities.Category> Categories { get; set; }

    public DbSet<Entities.Product> Products { get; set; }


}
