using Category.Controller;
using Category.Handler;

namespace Category;

public static class DependencyContainer
{
    public static IServiceCollection AddCategoryCore(this IServiceCollection services)
    {
        // Register handlers
        services.AddScoped<ICreateCategoryInputPort, CreateCategoryHandler>();
        services.AddScoped<IUpdateCategoryInputPort, UpdateCategoryHandler>();
        services.AddScoped<IGetCategoryInputPort, GetCategoryHandler>();
        services.AddScoped<IDeactivateCategoryInputPort, DeactivateCategoryHandler>();

        // Register controllers
        services.AddScoped<ICreateCategoryController, CreateCategoryController>();
        services.AddScoped<IUpdateCategoryController, UpdateCategoryController>();
        services.AddScoped<IGetCategoryController, GetCategoryController>();
        services.AddScoped<IDeactivateCategoryController, DeactivateCategoryController>();

        return services;
    }
}