using FluentValidation;

using Hotelium.Users.Dto;

namespace Hotelium.Users.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(u => u.Name).NotEmpty().MaximumLength(UserConsts.MaxNameLength);
        RuleFor(u => u.Surname).NotEmpty().MaximumLength(UserConsts.MaxSurnameLength);
        RuleFor(u => u.Email).NotEmpty().EmailAddress().MaximumLength(UserConsts.MaxEmailLength);
        RuleFor(u => u.Password).NotEmpty().MaximumLength(UserConsts.MaxPasswordLength);

        When(u => u.Address is not null, () => RuleFor(u => u.Address!).SetValidator(new AddressDtoValidator()));
    }
}