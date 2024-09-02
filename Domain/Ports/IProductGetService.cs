using Domain.Dto;

namespace Domain.Ports;

public interface IProductGetService
{
    public Task<IEnumerable<ProductDto>> GetProductsAsync();
    public Task<ProductDto?> GetSingleProductByIdAsync(Guid id);
}
