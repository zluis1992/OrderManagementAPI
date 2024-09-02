using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataSource.ModelConfig;

public class ORderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(b => b.Date).IsRequired();
        builder.Property(b => b.ProductId).IsRequired();
        builder.Property(b => b.Quantity).IsRequired();
        builder.Property(b => b.UnitValue).IsRequired();
        builder.Property(b => b.TotalValue).IsRequired();
    }
}
