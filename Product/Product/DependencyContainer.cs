using Microsoft.Extensions.DependencyInjection;
using Product.Controller;
using Product.Handler;
using Product.Interfaces;
using Product.Internals.Controllers;
using Product.Internals.InputPorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product;
public static class DependencyContainer
{
    public static IServiceCollection AddProductCore(this IServiceCollection services)
    {
        // Register handlers
        services.AddScoped<ICreateProductInputPort, CreateProductHandler>();
        services.AddScoped<IGetProductInputPort, GetProductHandler>();
        services.AddScoped<IUpdateProductInputPort, UpdateProductHandler>();
        services.AddScoped<IDesactivateProductInputPort, DesactivateProductHandler>();

        // Register controllers
        services.AddScoped<ICreateProductController, CreateProductController>();
        services.AddScoped<IGetProductController, GetProductController>();
        services.AddScoped<IUpdateProductController, UpdateProductController>();
        services.AddScoped<IDesactiveProductController, DesactivateProductController>();
        return services;
    }
};