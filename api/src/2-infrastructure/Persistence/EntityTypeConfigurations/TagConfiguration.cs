using Chores.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chores.Persistence.EntityTypeConfigurations;

internal sealed class TagConfiguration : AuditableEntityConfiguration<Tag>
{
    protected override void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder
            .HasIndex(t => t.Name)
            .IsUnique();
    }
}