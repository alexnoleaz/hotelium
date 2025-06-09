namespace Hotelium.Shared.Entities;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}