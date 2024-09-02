using Domain.Dto;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports;

namespace Domain.Services.Orders;

[DomainService]
public class OrderSaveService(
    IOrderRepository orderRepository,
    IProductGetService productGetService,
    IUnitOfWork unitOfWork)
{
    private async Task SaveChangesInUnitOfWorkAsync(CancellationToken cancellationToken)
    {
        await unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task<OrderDto> SaveOrderAsync(OrderRequestDto o, CancellationToken cancellationToken)
    {
        await ValidateOrder(o);
        var product = await productGetService.GetSingleProductByIdAsync(o.ProductId) ??
                      throw new OrderException("El producto enviado no existe");

        var order = await SaveOrderInRepositoryAsync(new Order(o.OrderDate, o.ProductId, o.Quantity, product.Price,
            product.Price * o.Quantity));
        await SaveChangesInUnitOfWorkAsync(cancellationToken);
        return new OrderDto(order.Id, order.Date, order.ProductId, product.Name, order.Quantity, order.UnitValue,
            order.TotalValue);
    }

    private async Task<Order> SaveOrderInRepositoryAsync(Order order)
    {
        return await orderRepository.SaveOrderAsync(order);
    }

    private static Task ValidateOrder(OrderRequestDto o)
    {
        ArgumentNullException.ThrowIfNull(o, nameof(o));
        return Task.CompletedTask;
    }
}
