using Category.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthWind.EFCore.Repositories.DbContexts;
using NorthWind.EFCore.Repositories.Options;
using NorthWind.EFCore.Repositories.Repositories.Categories;
using NorthWind.EFCore.Repositories.Repositories.Products;
using NorthWind.EFCore.Repositories.Repositories.Users;
using Product.Interfaces;
using User.Interfaces;

namespace NorthWind.EFCore.Repositories;
public static class DependencyContainer
{
    public static IServiceCollection AddNorthWIndRepositoriesSqlLite(this IServiceCollection services)
    {
        Action<DbContextOptionsBuilder> ConfigureOptions = options =>
        {
            options.UseSqlite("Data Source=northwinddatabase.db");
        };
        services.AddScoped<NorthWindSqlLiteDbContext>();
        services.AddNorthWindRepositories(ConfigureOptions);
        return services;
    }

    public static IServiceCollection AddNorthWindRepositorySqlServer(this IServiceCollection services, Action<NorthWindDbOptions> optionsDb)
    {
        NorthWindDbOptions Options = new();
        optionsDb(Options);
        Action<DbContextOptionsBuilder> ConfigureOptions = options =>
        {
            options.UseSqlServer(Options.ConnectionString);
        };
        services.AddNorthWindRepositories(ConfigureOptions);
        return services;
    }



    public static IServiceCollection AddNorthWindRepositories(this IServiceCollection services, Action<DbContextOptionsBuilder> configureOptions)
    {
        services.AddDbContext<CommandDbContext>(configureOptions);
        services.AddDbContext<QueryDbContext>(configureOptions);

        services.AddScoped<ICommandCategoryRepository, CommandCategoryRepository>();

        services.AddScoped<IQueryableCategoryRepository, QueryableCategoryRepository>();
        services.AddScoped<IQueryableCategoryReadRepository, QueryableCategoryReadRepository>();

        services.AddScoped<ICommandProductRepository, CommandProductRepository>();
        services.AddScoped<IQueryableProductRepository, QueryableProductRepository>();

        services.AddScoped<ICommandUserRepository, CommandUserRepository>();
        services.AddScoped<IQueryableUserRepository, QueryableUserRepository>();
        return services;

    }

    public static IHost InitializeSqlLiteDb(this IHost app)
    {
        using IServiceScope Scope = app.Services.CreateScope();
        var Context = Scope.ServiceProvider.GetRequiredService<NorthWindSqlLiteDbContext>();
        Context.Database.EnsureCreated();
        return app;
    }

}
