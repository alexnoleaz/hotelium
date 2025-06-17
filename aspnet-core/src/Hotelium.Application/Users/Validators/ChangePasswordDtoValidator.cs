using FluentValidation;

using Hotelium.Users.Dto;

namespace Hotelium.Users.Validators;

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{

    public ChangePasswordDtoValidator()
    {
        RuleFor(u => u.CurrentPassword).NotEmpty().MaximumLength(UserConsts.MaxPasswordLength);
        RuleFor(u => u.NewPassword).NotEmpty().MaximumLength(UserConsts.MaxPasswordLength);
    }
}