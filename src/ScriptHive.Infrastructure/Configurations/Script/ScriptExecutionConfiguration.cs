using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScriptHive.Domain.Entities.ScriptExecution;
using ScriptHive.Domain.ValueObjects.ExecutionStatus;

namespace ScriptHive.Infrastructure.Configurations.ScriptExecutionConfiguration;

public class ScriptExecutionConfiguration : IEntityTypeConfiguration<ScriptExecution>
{
    public void Configure(EntityTypeBuilder<ScriptExecution> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ScriptId)
            .IsRequired();

        builder.Property(c => c.Input)
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion(
                v => v.ToString(),
                v => ExecutionStatusHelper.ParseExecutionStatus(v))
            .IsRequired();

        builder.Property(c => c.Result);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.StartedAt);

        builder.Property(c => c.FinishedAt);
    }
}