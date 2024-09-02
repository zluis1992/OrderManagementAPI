using System.Linq.Expressions;
using Domain.Dto;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Ports;
using LinqKit;

namespace Infrastructure.Adapters.Repositories;

[Repository]
public class OrderRepository(IRepository<Order> dataSource) : IOrderRepository
{
    private readonly IRepository<Order> _dataSource = dataSource
                                                        ?? throw new ArgumentNullException(nameof(dataSource));

    public Task<IEnumerable<Order>> GetOrdersByFilterAsync(OrderFilterDto f)
    {
        var filter = BuildFilter(f);

        return _dataSource.GetManyAsync(filter);
    }

    public async Task<Order?> GetSingleOrderByIdAsync(Guid id)
    {
        return await _dataSource.GetOneAsync(id);
    }

    public async Task<Order> SaveOrderAsync(Order order)
    {
        return await _dataSource.AddAsync(order);
    }

    public async Task UpdateOrderAsync(Order order)
    {
        await _dataSource.UpdateAsync(order);
    }

    public async Task DeleteOrderAsync(Order order)
    {
        await _dataSource.DeleteAsync(order);
    }

    private static Expression<Func<Order, bool>> BuildFilter(OrderFilterDto f)
    {
        Expression<Func<Order, bool>> filter = o => true;

        if (f.Id.HasValue) filter = filter.And(o => o.Id == f.Id.Value);

        if (f.MinPrice.HasValue) filter = filter.And(o => o.TotalValue >= f.MinPrice.Value);

        if (f.MaxPrice.HasValue) filter = filter.And(o => o.TotalValue <= f.MaxPrice.Value);

        return filter;
    }
}
