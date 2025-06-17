using FluentValidation;

using Hotelium.Users.Dto;
using Hotelium.Users.Values;

namespace Hotelium.Users.Validators;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Street).NotEmpty().MaximumLength(AddressConsts.MaxStreetLength);
        RuleFor(x => x.City).NotEmpty().MaximumLength(AddressConsts.MaxCityLength);
    }
}