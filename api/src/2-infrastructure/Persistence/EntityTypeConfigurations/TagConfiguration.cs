using Chores.Domain.Models.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chores.Persistence.EntityTypeConfigurations;

internal sealed class TagConfiguration : AuditableEntityConfiguration<Tag>
{
    protected override void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder
            .HasIndex(t => new { t.Name, t.CreatedBy })
            .IsUnique();

        builder
            .Property(t => t.Color)
            .HasMaxLength(7)
            .IsFixedLength();

        builder
            .Property(t => t.Icon)
            .HasMaxLength(Tag.IconMaxLength);
    }
}