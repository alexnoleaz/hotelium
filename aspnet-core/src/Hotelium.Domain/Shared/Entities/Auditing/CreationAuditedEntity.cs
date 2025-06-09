namespace Hotelium.Shared.Entities.Auditing;

public abstract class CreationAuditedEntity : CreationAuditedEntity<int>, IEntity { }

public abstract class CreationAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>, IHasCreationTime
{
    public virtual DateTime CreationTime { get; set; } = DateTime.UtcNow;
}