using ScriptHive.Domain.Entities.User;
using ScriptHive.Domain.Interfaces.UserInterfaces;
using ScriptHive.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ScriptHive.Infrastructure.Repositories.UserRepository;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync()
        => await context.Users.AsNoTracking().ToListAsync();

    public async Task<User?> GetByIdAsync(Guid id)
    => await context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == id);

    public async Task CreateAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        context.Entry(user).Property(x => x.CreatedAt).IsModified = false;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null) return;
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Users.AnyAsync(c => c.Id == id);
    }
}
