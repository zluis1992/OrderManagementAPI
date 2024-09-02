using System.Net;
using System.Text.Json;
using Domain.Dto;
using Domain.Ports;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Adapters.Services.Products;

[InfrastructureService]
public class ProductGetService(HttpClient httpClient, IConfiguration configuration) : IProductGetService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly string? _productApiUrl = configuration["UrlProductAPI"];

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        var response = await httpClient.GetAsync($"{_productApiUrl}/product");

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        var products = JsonSerializer.Deserialize<IEnumerable<ProductDto>>(responseContent, JsonSerializerOptions);

        return products ?? [];
    }

    public async Task<ProductDto?> GetSingleProductByIdAsync(Guid id)
    {
        var response = await httpClient.GetAsync($"{_productApiUrl}/product/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        var product = JsonSerializer.Deserialize<ProductDto>(responseContent, JsonSerializerOptions);

        return product;
    }
}
