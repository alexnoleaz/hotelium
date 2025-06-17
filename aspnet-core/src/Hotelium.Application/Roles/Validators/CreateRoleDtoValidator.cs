using FluentValidation;

using Hotelium.Roles.Dto;

namespace Hotelium.Roles.Validators;

public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleDtoValidator() =>
        RuleFor(r => r.Name).NotEmpty().MaximumLength(RoleConsts.MaxNameLength);

}