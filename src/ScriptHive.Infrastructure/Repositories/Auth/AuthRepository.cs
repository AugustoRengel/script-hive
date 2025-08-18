using ScriptHive.Domain.Entities.User;
using ScriptHive.Domain.Interfaces.AuthInterfaces;
using ScriptHive.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ScriptHive.Infrastructure.Repositories.AuthRepository;

public class AuthRepository(AppDbContext context) : IAuthRepository
{
    private readonly AppDbContext _context = context;

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
    }
}
