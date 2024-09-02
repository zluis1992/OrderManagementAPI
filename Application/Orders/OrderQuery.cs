using Domain.Dto;
using MediatR;

namespace Application.Orders;

public record OrderQuery(
    Guid Id
    ) : IRequest<OrderDto?>;
