using Domain.Dto;
using MediatR;

namespace Application.Orders;

public record OrderSaveCommand(
    Guid? Id,
    DateOnly OrderDate,
    Guid ProductId,
    int Quantity
) : IRequest<OrderDto>;
