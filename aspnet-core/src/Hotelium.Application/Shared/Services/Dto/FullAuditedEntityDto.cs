using Hotelium.Shared.Entities.Auditing;

namespace Hotelium.Shared.Services.Dto;

public abstract class FullAuditedEntityDto : FullAuditedEntityDto<int> { }

public abstract class FullAuditedEntityDto<TPrimaryKey> : AuditedEntityDto<TPrimaryKey>, IHasDeletionTime
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletionTime { get; set; }
}