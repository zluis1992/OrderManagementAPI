using Domain.Dto;
using Domain.Services.Orders;
using MediatR;

namespace Application.Orders;

public class OrderSaveCommandHandler(OrderSaveService service) : IRequestHandler<OrderSaveCommand, OrderDto>
{
    private readonly OrderSaveService _service = service ?? throw new ArgumentNullException(nameof(service));

    public async Task<OrderDto> Handle(OrderSaveCommand request, CancellationToken cancellationToken)
    {
        var orderSaved = await _service.SaveOrderAsync(
            new OrderRequestDto(request.Id, request.OrderDate, request.ProductId, request.Quantity), cancellationToken
        );

        return orderSaved;
    }
}
