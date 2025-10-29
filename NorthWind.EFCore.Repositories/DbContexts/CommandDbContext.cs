using Microsoft.EntityFrameworkCore;

namespace NorthWind.EFCore.Repositories.DbContexts;
internal class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options)
    {
    }

    public DbSet<Entities.Category> Categories { get; set; }

}
