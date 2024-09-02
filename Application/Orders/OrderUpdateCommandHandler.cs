using Domain.Dto;
using Domain.Services.Orders;
using MediatR;

namespace Application.Orders;

public class OrderUpdateCommandHandler(OrderUpdateService service) : IRequestHandler<OrderUpdateCommand, bool>
{
    private readonly OrderUpdateService _service = service ?? throw new ArgumentNullException(nameof(service));

    public async Task<bool> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
    {
        return await _service.UpdateOrderAsync(
            new OrderRequestDto(request.Id, request.OrderDate, request.ProductId, request.Quantity), cancellationToken
        );
    }
}
