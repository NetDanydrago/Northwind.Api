using Microsoft.Extensions.DependencyInjection;
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

        // Register controllers
        services.AddScoped<ICreateProductController, CreateProductController>();

        return services;
    }
};