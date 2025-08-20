using ScriptHive.Domain.Entities.ScriptExecution;
using ScriptHive.Domain.Interfaces.ScriptInterfaces;
using ScriptHive.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;

namespace ScriptHive.Infrastructure.Repositories.ScriptExecutionRepository;

public class ScriptExecutionRepository(AppDbContext context) : IScriptExecutionRepository
{
    public async Task<IEnumerable<ScriptExecution>> GetAllAsync()
        => await context.ScriptExecutions.AsNoTracking().ToListAsync();

    public async Task<ScriptExecution?> GetByIdAsync(Guid id)
    => await context.ScriptExecutions
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == id);

    public async Task CreateAsync(ScriptExecution scriptExecution)
    {
        context.ScriptExecutions.Add(scriptExecution);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ScriptExecution scriptExecution)
    {
        context.ScriptExecutions.Update(scriptExecution);
        context.Entry(scriptExecution).Property(x => x.StartedAt).IsModified = false;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var scriptExecution = await context.ScriptExecutions.FindAsync(id);
        if (scriptExecution is null) return;
        context.ScriptExecutions.Remove(scriptExecution);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.ScriptExecutions.AnyAsync(c => c.Id == id);
    }
}
