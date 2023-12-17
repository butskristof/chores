using Chores.Domain.Models.Chores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chores.Persistence.EntityTypeConfigurations;

internal sealed class ChoreConfiguration : AuditableEntityConfiguration<Chore>
{
    protected override void Configure(EntityTypeBuilder<Chore> builder)
    {
    }
}