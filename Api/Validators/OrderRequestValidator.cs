using Application.Orders;
using FluentValidation;

namespace Api.Validators;

public class OrderRequestValidator : AbstractValidator<OrderSaveCommand>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.OrderDate).NotEmpty().WithMessage("The orderDate must not be empty.");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("The product must not be empty.");
        RuleFor(x => x.Quantity).NotEmpty().WithMessage("The quantity must not be empty.")
            .GreaterThanOrEqualTo(1).WithMessage("The quantity must be greater than or equal to 1.");
    }
}
