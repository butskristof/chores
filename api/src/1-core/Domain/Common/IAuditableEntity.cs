namespace Chores.Domain.Common;

public interface IAuditableEntity
{
    public string? CreatedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
}