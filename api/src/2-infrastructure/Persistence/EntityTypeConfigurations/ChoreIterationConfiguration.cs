using Chores.Domain.Models.Chores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chores.Persistence.EntityTypeConfigurations;

internal sealed class ChoreIterationConfiguration : IEntityTypeConfiguration<ChoreIteration>
{
    public void Configure(EntityTypeBuilder<ChoreIteration> builder)
    {
        builder
            .ToTable("ChoreIterations");

        builder
            .HasOne<Chore>()
            .WithMany(c => c.Iterations)
            .HasForeignKey(ci => ci.ChoreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}