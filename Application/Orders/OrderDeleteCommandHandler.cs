using Domain.Services.Orders;
using MediatR;

namespace Application.Orders;

public class OrderDeleteCommandHandler(OrderDeleteService service) : IRequestHandler<OrderDeleteCommand, bool>
{
    private readonly OrderDeleteService _service = service ?? throw new ArgumentNullException(nameof(service));

    public async Task<bool> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
    {
        return await _service.DeleteOrderAsync(request.Id, cancellationToken);
    }
}
