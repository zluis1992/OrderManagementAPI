using System.Text.Json;
using Application.Orders;
using Domain.Dto;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests;

public class OrderApiTest
{
    [Fact]
    public async Task DeleteOrderSuccess()
    {
        await using var webApp = new ApiApp();
        var serviceCollection = webApp.GetServiceCollection();
        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var order = new Order(DateOnly.MinValue, Guid.NewGuid(), 1, 2, 2);
        var orderCreated = await repository.AddAsync(order);
        await unitOfWork.SaveAsync(new CancellationTokenSource().Token);

        var client = webApp.CreateClient();
        var deleteResponse = await client.DeleteAsync($"/api/order/{orderCreated.Id}");

        deleteResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task GetSingleOrderSuccess()
    {
        await using var webApp = new ApiApp();
        var serviceCollection = webApp.GetServiceCollection();
        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var order = new Order(DateOnly.MinValue, Guid.NewGuid(), 1, 2, 2);
        var orderCreated = await repository.AddAsync(order);
        await unitOfWork.SaveAsync(new CancellationTokenSource().Token);

        var client = webApp.CreateClient();
        var singleOrder = await client.GetFromJsonAsync<OrderDto>($"/api/order/{orderCreated.Id}");

        Assert.True(singleOrder is not null);
        Assert.Equal(singleOrder.Id, orderCreated.Id);
    }

    [Fact]
    public async Task PostOrderSuccess()
    {
        await using var webApp = new ApiApp();

        OrderSaveCommand orderCommand = new(null, DateOnly.MinValue, Guid.NewGuid(), 1);
        var client = webApp.CreateClient();
        var request = await client.PostAsJsonAsync("/api/order/", orderCommand);

        request.EnsureSuccessStatusCode();

        var deserializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var responseData =
            JsonSerializer.Deserialize<OrderDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);

        Assert.True(responseData is not null);
        Assert.IsType<OrderDto>(responseData);
    }
}
