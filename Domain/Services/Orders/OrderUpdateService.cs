using Domain.Dto;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports;

namespace Domain.Services.Orders;

[DomainService]
public class OrderUpdateService(
    IOrderRepository orderRepository,
    IProductGetService productGetService,
    IUnitOfWork unitOfWork)
{
    private async Task SaveChangesInUnitOfWorkAsync(CancellationToken cancellationToken)
    {
        await unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task<bool> UpdateOrderAsync(OrderRequestDto updatedOrder, CancellationToken cancellationToken)
    {
        await ValidateOrder(updatedOrder);

        var existingOrder = await orderRepository.GetSingleOrderByIdAsync((Guid)updatedOrder.Id!);
        if (existingOrder == null) return false;
        var product = await productGetService.GetSingleProductByIdAsync(updatedOrder.ProductId) ??
                      throw new OrderException("El producto enviado no existe");

        existingOrder.Date = updatedOrder.OrderDate;
        existingOrder.ProductId = updatedOrder.ProductId;
        existingOrder.Quantity = updatedOrder.Quantity;
        existingOrder.UnitValue = product.Price;
        existingOrder.TotalValue = product.Price * updatedOrder.Quantity;

        await UpdateOrderInRepositoryAsync(existingOrder);
        await SaveChangesInUnitOfWorkAsync(cancellationToken);

        return true;
    }

    private async Task UpdateOrderInRepositoryAsync(Order order)
    {
        await orderRepository.UpdateOrderAsync(order);
    }

    private static Task ValidateOrder(OrderRequestDto o)
    {
        ArgumentNullException.ThrowIfNull(o, nameof(o));
        return Task.CompletedTask;
    }
}
