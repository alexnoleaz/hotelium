using FluentValidation;

using Hotelium.Shared.Services.Dto;

namespace Hotelium.Shared.Services.Validators;

public class LimitedResultRequestDtoValidator : AbstractValidator<LimitedResultRequestDto>
{
    public LimitedResultRequestDtoValidator()
    {
        RuleFor(x => x.MaxResultCount).InclusiveBetween(1, int.MaxValue);
    }
}