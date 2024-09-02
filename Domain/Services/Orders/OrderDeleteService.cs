using Domain.Entities;
using Domain.Ports;

namespace Domain.Services.Orders;

[DomainService]
public class OrderDeleteService(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
{
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<bool> DeleteOrderAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var existingOrder = await _orderRepository.GetSingleOrderByIdAsync(orderId);
        if (existingOrder == null) return false;

        existingOrder.Active = false;
        await DeleteSoftOrderFromRepositoryAsync(existingOrder);
        await SaveChangesInUnitOfWorkAsync(cancellationToken);

        return true;
    }

    private async Task DeleteSoftOrderFromRepositoryAsync(Order order)
    {
        await _orderRepository.UpdateOrderAsync(order);
    }

    private async Task SaveChangesInUnitOfWorkAsync(CancellationToken cancellationToken)
    {
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
