using Domain.Dto;
using Domain.Entities;

namespace Domain.Ports;

public interface IOrderRepository
{
    public Task DeleteOrderAsync(Order order);
    public Task<IEnumerable<Order>> GetOrdersByFilterAsync(OrderFilterDto f);
    public Task<Order?> GetSingleOrderByIdAsync(Guid id);
    public Task<Order> SaveOrderAsync(Order order);
    public Task UpdateOrderAsync(Order order);
}
