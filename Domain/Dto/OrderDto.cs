namespace Domain.Dto;

public record OrderDto(
    Guid Id,
    DateOnly OrderDate,
    Guid? ProductId,
    string? ProductName,
    int Quantity,
    decimal UnitValue,
    decimal TotalValue);
