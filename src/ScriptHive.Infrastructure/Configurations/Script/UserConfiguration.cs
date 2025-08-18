using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScriptHive.Domain.Entities.User;
using ScriptHive.Domain.ValueObjects.UserRole;

namespace ScriptHive.Infrastructure.Configurations.UserConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Role)
            .HasConversion(
                v => v.ToString(),
                v => UserRoleHelper.ParseUserRole(v))
            .IsRequired();

        builder.Property(c => c.PasswordHash)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();
    }
}
