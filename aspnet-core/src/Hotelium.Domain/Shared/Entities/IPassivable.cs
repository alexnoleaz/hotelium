namespace Hotelium.Shared.Entities;

public interface IPassivable
{
    bool IsActive { get; set; }
}