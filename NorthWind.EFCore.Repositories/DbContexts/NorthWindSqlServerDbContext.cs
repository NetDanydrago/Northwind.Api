using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.EFCore.Repositories.DbContexts;
internal class NorthWindSqlServerDbContext : DbContext
{
    //Add-Migration InitialCreate -Project NorthWind.EFCore.Repositories -StartupProject NorthWind.EFCore.Repositories -Context NorthWindSqlServerDbContext

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=NorthwindDB;Trusted_Connection=True;");
    }

    public DbSet<Entities.Category> Categories { get; set; }

    public DbSet<Entities.Product> Products { get; set; }

    public DbSet<Entities.User> Users { get; set; }
}
