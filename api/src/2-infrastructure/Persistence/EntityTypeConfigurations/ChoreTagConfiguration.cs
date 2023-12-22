using Chores.Domain.Models.Chores;
using Chores.Domain.Models.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chores.Persistence.EntityTypeConfigurations;

internal sealed class ChoreTagConfiguration : IEntityTypeConfiguration<ChoreTag>
{
    public void Configure(EntityTypeBuilder<ChoreTag> builder)
    {
        builder
            .ToTable("ChoreTags");

        builder
            .HasKey(ct => new { ct.ChoreId, ct.TagId });

        builder
            .HasOne<Chore>()
            .WithMany()
            .HasForeignKey(ct => ct.ChoreId);

        builder
            .HasOne<Tag>()
            .WithMany()
            .HasForeignKey(ct => ct.TagId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}