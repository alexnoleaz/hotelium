namespace Hotelium.Shared.Services.Dto;

public interface IListResult<T>
{
    IReadOnlyList<T> Items { get; set; }
}