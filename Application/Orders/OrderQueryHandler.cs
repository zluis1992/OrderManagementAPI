using Domain.Dto;
using Domain.Services.Orders;
using MediatR;

namespace Application.Orders;

public class OrderQueryHandler(OrderGetService service) : IRequestHandler<OrderQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(OrderQuery request, CancellationToken cancellationToken)
    {
        return await service.GetSingleOrderByIdAsync(request.Id);
    }
}
