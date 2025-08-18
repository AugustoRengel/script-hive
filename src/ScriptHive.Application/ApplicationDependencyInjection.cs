using ScriptHive.Application.Interfaces.ScriptInterfaces;
using ScriptHive.Application.Interfaces.UserInterfaces;

using ScriptHive.Application.Services.ScriptServices;
using ScriptHive.Application.Services.UserServices;

using ScriptHive.Application.Validators.ScriptValidator;
using ScriptHive.Application.Validators.UserValidator;

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ScriptHive.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<UserValidator>();
        services.AddValidatorsFromAssemblyContaining<ScriptValidator>();

        // Register application services here
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IScriptService, ScriptService>();
        return services;
    }
}
