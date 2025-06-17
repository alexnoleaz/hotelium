using Hotelium.Shared.Values;

namespace Hotelium.Users.Values;

public class Address : ValueObject
{
    private readonly string _street;
    private readonly string _city;

    public Address(string street, string city)
    {
        _street = street;
        _city = city;
    }

    public string Street => _street;
    public string City => _city;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Street;
        yield return City;
    }
}