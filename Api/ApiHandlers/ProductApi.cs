using Api.Filters;
using Api.Validators;
using Application.Orders;
using Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.ApiHandlers;

public static class OrderApi
{
    public static RouteGroupBuilder MapOrder(this IEndpointRouteBuilder routeHandler)
    {
        //GetById
        routeHandler.MapGet("/{id}",
                async (IMediator mediator, Guid id) =>
                {
                    var order = await mediator.Send(new OrderQuery(id));
                    return order is null ? Results.NotFound() : Results.Ok(order);
                })
            .Produces(StatusCodes.Status200OK, typeof(OrderDto))
            .Produces(StatusCodes.Status404NotFound);

        //Save
        routeHandler.MapPost("/", async (IMediator mediator, [Validate] OrderSaveCommand command) =>
            {
                var order = await mediator.Send(command);
                return Results.Created(new Uri($"/api/order/{order.Id}", UriKind.Relative), order);
            })
            .Produces(StatusCodes.Status201Created);

        // GetByFilter
        routeHandler.MapGet("/", async (IMediator mediator,
                [FromQuery] Guid? id,
                [FromQuery] decimal? minValue,
                [FromQuery] decimal? maxValue,
                [FromServices] OrderQueryFilterValidator validator) =>
            {
                var orderQueryFilter = new OrderQueryFilter(id, minValue, maxValue);

                var validationResult = await validator.ValidateAsync(orderQueryFilter);

                if (!validationResult.IsValid) return Results.UnprocessableEntity(validationResult.Errors);

                var orders = await mediator.Send(orderQueryFilter);
                return Results.Ok(orders);
            })
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<OrderDto>));

        // Update
        routeHandler.MapPut("/{id}", async (IMediator mediator, Guid id, [Validate] OrderUpdateCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("The order ID in the path does not match the ID in the body.");

                var result = await mediator.Send(command);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        // Delete
        routeHandler.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
            {
                var result = await mediator.Send(new OrderDeleteCommand(id));
                return result ? Results.NoContent() : Results.NotFound();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);


        return (RouteGroupBuilder)routeHandler;
    }
}
