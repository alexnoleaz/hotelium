using FluentValidation;

using Hotelium.Shared.Services.Dto;

namespace Hotelium.Shared.Services.Validators;

public class PagedResultRequestDtoValidator : AbstractValidator<PagedResultRequestDto>
{
    public PagedResultRequestDtoValidator()
    {
        Include(new LimitedResultRequestDtoValidator());

        RuleFor(x => x.SkipCount).InclusiveBetween(0, int.MaxValue);
    }
}