namespace Hotelium.Shared.Services.Dto;

public interface ILimitedResultRequest
{
    int MaxResultCount { get; set; }
}