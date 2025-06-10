using Hotelium.Shared.Entities.Auditing;

namespace Hotelium.Shared.Services.Dto;

public abstract class AuditedEntityDto : AuditedEntityDto<int> { }

public abstract class AuditedEntityDto<TPrimaryKey> : CreationAuditedEntityDto<TPrimaryKey>, IHasModificationTime
{
    public DateTime? LastModificationTime { get; set; }
}