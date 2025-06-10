using Hotelium.Shared.Entities.Auditing;

namespace Hotelium.Shared.Services.Dto;

public abstract class CreationAuditedEntityDto : CreationAuditedEntityDto<int> { }

public abstract class CreationAuditedEntityDto<TPrimaryKey> : EntityDto<TPrimaryKey>, IHasCreationTime
{
    public DateTime CreationTime { get; set; } = DateTime.UtcNow;
}