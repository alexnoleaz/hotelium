namespace Hotelium.Shared.Entities.Auditing;

public interface IHasDeletionTime : ISoftDelete
{
    DateTime? DeletionTime { get; set; }
}