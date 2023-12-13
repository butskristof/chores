namespace Chores.Domain.Common;

public abstract class BaseAuditableEntity : IAuditableEntity
{
    public Guid Id { get; init; }
    
    public string? CreatedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
}