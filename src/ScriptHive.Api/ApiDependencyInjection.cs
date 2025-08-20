using FluentValidation;
using ScriptHive.Api.Validators.ScriptValidator;

namespace ScriptHive.Api;


public static class ApiDependencyInjection
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<ScriptValidator>();

        // Register application services here
        // ...
        return services;
    }
}
