using GeminiOrderFulfillment.Domain.FulfillmentAggregate.Entities;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeminiOrderFulfillment.Infrastructure.Persistence.Configurations;

public sealed class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
{
    public void Configure(EntityTypeBuilder<LineItem> builder)
    {
        builder.ToTable("LineItems");

        builder.HasKey(li => li.Id);
        builder.Property(li => li.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => LineItemId.Create(value));

        builder.Property(li => li.ProductId)
            .IsRequired();

        builder.Property(li => li.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(li => li.Quantity)
            .IsRequired();
    }

}