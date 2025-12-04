using Microsoft.EntityFrameworkCore;

namespace NorthWind.EFCore.Repositories.DbContexts;
internal class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options)
    {
    }

    public DbSet<Entities.Category> Categories { get; set; }

    public DbSet<Entities.Product> Products { get; set; }

    public DbSet<Entities.User> Users { get; set; }


}
