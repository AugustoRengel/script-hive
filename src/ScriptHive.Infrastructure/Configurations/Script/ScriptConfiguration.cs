using ScriptHive.Domain.Entities.Script;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScriptHive.Infrastructure.Configurations.ScriptConfiguration;

public class ScriptConfiguration : IEntityTypeConfiguration<Script>
{
    public void Configure(EntityTypeBuilder<Script> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Content)
            .IsRequired();

        builder.Property(c => c.OwnerId)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();
    }
}
