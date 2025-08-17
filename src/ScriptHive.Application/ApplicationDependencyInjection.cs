using ScriptHive.Application.Interfaces.ScriptInterfaces;
using ScriptHive.Application.Services.ScriptServices;
using ScriptHive.Application.Validators.ScriptValidator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ScriptHive.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<ScriptValidator>();

        // Register application services here
        services.AddScoped<IScriptService, ScriptService>();
        return services;
    }
}
