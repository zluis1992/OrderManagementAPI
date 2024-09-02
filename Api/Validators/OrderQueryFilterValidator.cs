using System.Globalization;
using Application.Orders;
using FluentValidation;

namespace Api.Validators;

public class OrderQueryFilterValidator : AbstractValidator<OrderQueryFilter>
{
    public OrderQueryFilterValidator()
    {
        RuleFor(x => x.Id)
            .Must(value => value == null || Guid.TryParse(value.ToString(), CultureInfo.InvariantCulture, out _))
            .WithMessage(string.Format(CultureInfo.InvariantCulture, Resources.validationRuleMessage, "Id", "Guid"));

        RuleFor(x => x.MinValue)
            .Must(value => value is null or >= 0)
            .WithMessage("MinValue must be a non-negative number.");

        RuleFor(x => x.MaxValue)
            .Must(value => value is null or >= 0)
            .WithMessage("MaxValue must be a non-negative number.");

        RuleFor(x => x)
            .Must(x => !x.MinValue.HasValue || !x.MaxValue.HasValue || x.MaxValue >= x.MinValue)
            .WithMessage("MaxValue must be greater than or equal to MinValue.");

        RuleFor(x => x)
            .Must(x => !x.MinValue.HasValue || !x.MaxValue.HasValue || x.MinValue <= x.MaxValue)
            .WithMessage("MinValue must be less than or equal to MaxValue.");
    }
}
