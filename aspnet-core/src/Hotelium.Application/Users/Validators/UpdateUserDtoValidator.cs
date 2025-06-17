using FluentValidation;

using Hotelium.Users.Dto;

namespace Hotelium.Users.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(u => u.Name).NotEmpty().MaximumLength(UserConsts.MaxNameLength);
        RuleFor(u => u.Surname).NotEmpty().MaximumLength(UserConsts.MaxSurnameLength);
        RuleFor(u => u.Email).NotEmpty().EmailAddress().MaximumLength(UserConsts.MaxEmailLength);

        When(u => u.Address is not null, () => RuleFor(u => u.Address!).SetValidator(new AddressDtoValidator()));
    }
}