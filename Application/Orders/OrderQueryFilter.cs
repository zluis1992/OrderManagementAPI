using Domain.Dto;
using MediatR;

namespace Application.Orders;

public record OrderQueryFilter(
    Guid? Id = null,
    decimal? MinValue = null,
    decimal? MaxValue = null
) : IRequest<IEnumerable<OrderDto>>;
