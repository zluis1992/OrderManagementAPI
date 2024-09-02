namespace Domain.Entities;

public sealed class Order(DateOnly date, Guid productId, int quantity, decimal unitValue, decimal totalValue)
    : DomainEntity
{
    public DateOnly Date { get; set; } = date;
    public Guid ProductId { get; set; } = productId;
    public int Quantity { get; set; } = quantity;
    public decimal UnitValue { get; set; } = unitValue;
    public decimal TotalValue { get; set; } = totalValue;
    public bool Active { get; set; } = true;
}