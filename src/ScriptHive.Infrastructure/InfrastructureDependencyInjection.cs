using ScriptHive.Domain.Interfaces.ScriptInterfaces;
using Microsoft.Extensions.DependencyInjection;
using ScriptHive.Infrastructure.Repositories.ScriptRepository;

namespace ScriptHive.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register infrastructure services here
        services.AddScoped<IScriptRepository, ScriptRepository>();

        return services;
    }
}
