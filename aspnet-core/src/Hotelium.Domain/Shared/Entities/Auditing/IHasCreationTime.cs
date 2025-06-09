namespace Hotelium.Shared.Entities.Auditing;

public interface IHasCreationTime
{
    DateTime CreationTime { get; set; }
}