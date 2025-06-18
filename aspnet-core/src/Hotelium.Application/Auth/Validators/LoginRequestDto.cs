using FluentValidation;

using Hotelium.Auth.Dto;
using Hotelium.Users;

namespace Hotelium.Auth.Validators;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(UserConsts.MaxEmailLength);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(UserConsts.MaxPasswordLength);
    }
}