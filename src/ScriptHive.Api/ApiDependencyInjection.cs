using FluentValidation;
using ScriptHive.Api.Validators.ScriptRequestValidator;
using ScriptHive.Api.Validators.ScriptExecutionValidator;

namespace ScriptHive.Api;


public static class ApiDependencyInjection
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<ScriptRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ScriptExecutionValidator>();

        // Register application services here
        // ...
        return services;
    }
}
