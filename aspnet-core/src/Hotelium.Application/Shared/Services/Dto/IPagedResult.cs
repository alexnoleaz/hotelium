namespace Hotelium.Shared.Services.Dto;

public interface IPagedResult<T> : IListResult<T>, IHasTotalCount { }