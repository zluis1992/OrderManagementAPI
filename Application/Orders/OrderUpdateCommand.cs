using MediatR;

namespace Application.Orders;

public record OrderUpdateCommand(
    Guid Id,
    DateOnly OrderDate,
    Guid ProductId,
    int Quantity
) : IRequest<bool>;
