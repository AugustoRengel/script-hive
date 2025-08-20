using ScriptHive.Domain.Entities.Script;
using ScriptHive.Domain.Interfaces.ScriptInterfaces;
using ScriptHive.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ScriptHive.Infrastructure.Repositories.ScriptRepository;

public class ScriptRepository(AppDbContext context) : IScriptRepository
{
    public async Task<IEnumerable<Script>> GetAllAsync()
        => await context.Scripts.AsNoTracking().ToListAsync();

    public async Task<Script?> GetByIdAsync(Guid id)
    => await context.Scripts
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == id);

    public async Task CreateAsync(Script script)
    {
        context.Scripts.Add(script);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Script script)
    {
        context.Scripts.Update(script);
        context.Entry(script).Property(x => x.CreatedAt).IsModified = false;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var script = await context.Scripts.FindAsync(id);
        if (script is null) return;
        context.Scripts.Remove(script);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Scripts.AnyAsync(c => c.Id == id);
    }
}
