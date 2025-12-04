using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Controller;
using User.Handler;
using User.Internals.Controllers;
using User.Internals.InputPorts;

namespace User;

public static class DependencyContainer
{
    public static IServiceCollection AddUserCore(this IServiceCollection services)
    {
        //Handlers
        services.AddScoped<ICreateUserInputPort, CreateUserHandler>();
        services.AddScoped<IDesactivateUserInputPort, DesactivateUserHandler>();
        services.AddScoped<IGetUserInputPort, GetUserHandler>();
        services.AddScoped<IGetAllUsersInputPort, GetAllUsersHandler>();
        services.AddScoped<IUpdateUserInputPort, UpdateUserHandler>();
        //Controllers
        services.AddScoped<ICreateUserController, CreateUserController>();
        services.AddScoped<IDesactivateUserController, DesactivateUserController>();
        services.AddScoped<IGetAllUsersController, GetAllUsersController>();
        services.AddScoped<IGetUserController, GetUserController>();
        services.AddScoped<IUpdateUserController, UpdateUserController>();

        return services;
    }
}
