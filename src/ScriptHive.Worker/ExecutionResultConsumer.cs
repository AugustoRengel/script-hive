using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScriptHive.Domain.Interfaces.ScriptInterfaces;

namespace ScriptHive.Worker;

public class ExecutionResultConsumer : BackgroundService
{
    private readonly IScriptExecutionResultQueue _queue;
    private readonly IServiceProvider _serviceProvider;

    public ExecutionResultConsumer(IScriptExecutionResultQueue queue, IServiceProvider serviceProvider)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var execution in _queue.DequeueAllAsync(stoppingToken))
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IScriptExecutionRepository>();
            await repo.UpdateAsync(execution);
        }
    }
}
