namespace Hotelium.Shared.Services.Dto;

public class EntityDto : EntityDto<int>, IEntityDto { }

public class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
{
    public TPrimaryKey Id { get; set; }
}