using GeminiOrderFulfillment.Domain.FulfillmentAggregate;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeminiOrderFulfillment.Infrastructure.Persistence.Configurations;

public sealed class FulfillmentConfiguration : IEntityTypeConfiguration<Fullfillment>
{
    public void Configure(EntityTypeBuilder<Fullfillment> builder)
    {
        builder.ToTable("Fulfillments");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => FulfillmentId.Create(value));

        builder.Property(f => f.OrderId)
            .IsRequired();

        builder.Property(f => f.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Fulfillment_Status",
            "\"Status\" IN ('AWAITING_FULFILLMENT', 'TASK_CREATED', 'PICKING_IN_PROGRESS', 'PACKED', 'LABEL_GENERATED', 'SHIPPED', 'IN_TRANSIT', 'DELIVERED')"));

        builder.Property(f => f.TrackingNumber)
            .HasMaxLength(100);

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        builder.Property(f => f.UpdatedAt);
    }
}