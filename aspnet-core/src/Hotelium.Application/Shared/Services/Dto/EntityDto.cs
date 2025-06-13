namespace Hotelium.Shared.Services.Dto;

public class EntityDto : EntityDto<int>, IEntityDto
{
    public EntityDto() { }

    public EntityDto(int id) : base(id) { }
}

public class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
{
    public TPrimaryKey Id { get; set; }

    public EntityDto() { }

    public EntityDto(TPrimaryKey id) => Id = id;
}