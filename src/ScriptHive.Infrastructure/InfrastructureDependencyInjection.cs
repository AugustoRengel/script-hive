using ScriptHive.Domain.Interfaces.ScriptInterfaces;
using ScriptHive.Domain.Interfaces.UserInterfaces;
using ScriptHive.Domain.Interfaces.AuthInterfaces;

using ScriptHive.Infrastructure.Repositories.ScriptRepository;
using ScriptHive.Infrastructure.Repositories.UserRepository;
using ScriptHive.Infrastructure.Repositories.AuthRepository;

using Microsoft.Extensions.DependencyInjection;


namespace ScriptHive.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        // Register infrastructure services here
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IScriptRepository, ScriptRepository>();

        return services;
    }
}
