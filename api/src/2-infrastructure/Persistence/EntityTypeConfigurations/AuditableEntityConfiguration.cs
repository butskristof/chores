using Chores.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chores.Persistence.EntityTypeConfigurations;

internal abstract class AuditableEntityConfiguration<T> : IEntityTypeConfiguration<T>
    where T : class, IAuditableEntity
{
    void IEntityTypeConfiguration<T>.Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .Property(e => e.CreatedBy)
            .IsRequired();

        builder
            .Property(e => e.LastModifiedBy)
            .IsRequired();

        Configure(builder);
    }

    protected abstract void Configure(EntityTypeBuilder<T> builder);
}