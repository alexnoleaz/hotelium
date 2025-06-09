namespace Hotelium.Shared.Entities.Auditing;

public interface IHasModificationTime
{
    DateTime? LastModificationTime { get; set; }
}