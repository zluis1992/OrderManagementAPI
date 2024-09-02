using MediatR;

namespace Application.Orders;

public record OrderDeleteCommand(
    Guid Id) : IRequest<bool>;
