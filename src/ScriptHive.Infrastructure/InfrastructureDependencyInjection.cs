using Microsoft.Extensions.DependencyInjection;
using ScriptHive.Domain.Interfaces.AuthInterfaces;
using ScriptHive.Domain.Interfaces.ScriptInterfaces;
using ScriptHive.Domain.Interfaces.UserInterfaces;
using ScriptHive.Infrastructure.Queues;
using ScriptHive.Infrastructure.Repositories.AuthRepository;
using ScriptHive.Infrastructure.Repositories.ScriptExecutionRepository;
using ScriptHive.Infrastructure.Repositories.ScriptRepository;
using ScriptHive.Infrastructure.Repositories.UserRepository;


namespace ScriptHive.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        // Register infrastructure services here
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IScriptRepository, ScriptRepository>();
        services.AddScoped<IScriptExecutionRepository, ScriptExecutionRepository>();

        services.AddSingleton<IScriptExecutionResultQueue, ScriptExecutionResultQueue>();

        return services;
    }
}
