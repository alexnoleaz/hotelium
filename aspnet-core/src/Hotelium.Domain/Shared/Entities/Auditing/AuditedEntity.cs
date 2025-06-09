namespace Hotelium.Shared.Entities.Auditing;

public abstract class AuditedEntity : AuditedEntity<int>, IEntity { }

public abstract class AuditedEntity<TPrimaryKey>
    : CreationAuditedEntity<TPrimaryKey>,
        IHasModificationTime
{
    public virtual DateTime? LastModificationTime { get; set; }
}