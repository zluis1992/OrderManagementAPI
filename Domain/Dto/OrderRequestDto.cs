namespace Domain.Dto;

public record OrderRequestDto(
    Guid? Id,
    DateOnly OrderDate,
    Guid ProductId,
    int Quantity
);
