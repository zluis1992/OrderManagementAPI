using Domain.Dto;
using Domain.Ports;

namespace Domain.Services.Orders;

[DomainService]
public class OrderGetService(IOrderRepository orderRepository, IProductGetService productGetService)
{
    public async Task<IEnumerable<OrderDto>> GetOrdersByFilterAsync(OrderFilterDto f)
    {
        var orders = (await orderRepository.GetOrdersByFilterAsync(f)).ToList();
        if (orders.Count == 0) return [];
        var products = await productGetService.GetProductsAsync();

        var lOrderDto = orders.Where(x => x.Active).Select(order =>
        {
            var product = products.FirstOrDefault(p => p.Id == order.ProductId);

            return new OrderDto
            (
                order.Id,
                order.Date,
                order.ProductId,
                product?.Name,
                order.Quantity,
                order.UnitValue,
                order.TotalValue
            );
        }).ToList();

        return lOrderDto;
    }

    public async Task<OrderDto?> GetSingleOrderByIdAsync(Guid orderId)
    {
        var order = await orderRepository.GetSingleOrderByIdAsync(orderId);
        if (order is null) return null;
        var product = await productGetService.GetSingleProductByIdAsync(order.ProductId);
        return new OrderDto(order.Id,
            order.Date,
            order.Id,
            product?.Name,
            order.Quantity,
            order.UnitValue,
            order.TotalValue);
    }
}
