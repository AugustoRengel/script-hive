using ScriptHive.Application.Interfaces.ScriptInterfaces;
using ScriptHive.Application.Interfaces.UserInterfaces;

using ScriptHive.Application.Services.ScriptServices;
using ScriptHive.Application.Services.UserServices;

using ScriptHive.Application.Validators.ScriptValidator;
using ScriptHive.Application.Validators.UserValidator;
using ScriptHive.Application.Validators.AuthValidator;

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ScriptHive.Application.Interfaces.AuthInterfaces;
using ScriptHive.Application.Services.AuthServices;

namespace ScriptHive.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<UserValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginValidator>();
        services.AddValidatorsFromAssemblyContaining<ScriptValidator>();

        // Register application services here
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IScriptService, ScriptService>();
        return services;
    }
}
