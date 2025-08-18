using Microsoft.EntityFrameworkCore;
using ScriptHive.Domain.Entities.User;
using ScriptHive.Domain.ValueObjects.UserRole;
using ScriptHive.Infrastructure.Context;

namespace ScriptHive.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        context.Database.Migrate();

        if (!context.Users.Any(u => u.Email == "admin@system.local"))
        {
            var admin = new User(
                id: Guid.NewGuid(),
                name: "Admin",
                email: "admin@system.local",
                role: UserRole.Admin,
                passwordHash: "M4IjrM39pQP0lqOAGLoMJQ==.NCyGF5vWDjZDEG1GkAl9OU7YhGYLVVAkeMF16+GUhDg=",
                createdAt: DateTime.UtcNow
            );

            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}
