using Domain.Dto;
using Domain.Services.Orders;
using MediatR;

namespace Application.Orders;

public class OrderQueryFilterHandler(OrderGetService service)
    : IRequestHandler<OrderQueryFilter, IEnumerable<OrderDto>>
{
    public async Task<IEnumerable<OrderDto>> Handle(OrderQueryFilter request, CancellationToken cancellationToken)
    {
        var filter = new OrderFilterDto(
            request.Id,
            request.MinValue,
            request.MaxValue
        );

        return await service.GetOrdersByFilterAsync(filter);
    }
}
