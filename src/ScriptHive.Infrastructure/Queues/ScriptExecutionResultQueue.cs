using ScriptHive.Domain.Entities.ScriptExecution;
using ScriptHive.Domain.Interfaces.ScriptInterfaces;
using System.Threading.Channels;

namespace ScriptHive.Infrastructure.Queues;

public class ScriptExecutionResultQueue : IScriptExecutionResultQueue
{
    private readonly Channel<ScriptExecution> _channel = Channel.CreateUnbounded<ScriptExecution>();

    public Task EnqueueAsync(ScriptExecution execution)
    {
        return _channel.Writer.WriteAsync(execution).AsTask();
    }

    public IAsyncEnumerable<ScriptExecution> DequeueAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
