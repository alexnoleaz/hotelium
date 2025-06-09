namespace Hotelium.Shared.Entities.Auditing;

public abstract class FullAuditedEntity : FullAuditedEntity<int>, IEntity { }

public abstract class FullAuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey>, IHasDeletionTime
{
    public virtual bool IsDeleted { get; set; }

    public virtual DateTime? DeletionTime { get; set; }
}