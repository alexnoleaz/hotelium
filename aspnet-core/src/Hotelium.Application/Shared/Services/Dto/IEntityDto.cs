namespace Hotelium.Shared.Services.Dto;

public interface IEntityDto : IEntityDto<int> { }

public interface IEntityDto<TPrimaryKey>
{
    TPrimaryKey Id { get; set; }
}