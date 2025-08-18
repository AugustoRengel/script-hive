using ScriptHive.Domain.Interfaces.ScriptInterfaces;
using ScriptHive.Domain.Interfaces.UserInterfaces;

using ScriptHive.Infrastructure.Repositories.ScriptRepository;
using ScriptHive.Infrastructure.Repositories.UserRepository;

using Microsoft.Extensions.DependencyInjection;


namespace ScriptHive.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register infrastructure services here
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IScriptRepository, ScriptRepository>();

        return services;
    }
}
