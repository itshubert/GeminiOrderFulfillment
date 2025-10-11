using GeminiOrderFulfillment.Domain.FulfillmentAggregate;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeminiOrderFulfillment.Infrastructure.Persistence.Configurations;

public sealed class FulfillmentConfiguration : IEntityTypeConfiguration<Fulfillment>
{
    public void Configure(EntityTypeBuilder<Fulfillment> builder)
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

        builder.HasIndex(f => f.OrderId)
            .IsUnique();

        builder.Property(f => f.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Fulfillment_Status",
        "\"Status\" IN ('AWAITING_FULFILLMENT', 'TASK_CREATED', 'PICKING_IN_PROGRESS', 'PACKED', 'LABEL_GENERATED', 'ORDER_SHIPPED', 'IN_TRANSIT', 'DELIVERED')"));

        builder.Property(f => f.TrackingNumber)
            .HasMaxLength(100);

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        builder.Property(f => f.UpdatedAt);

        builder.OwnsOne(o => o.ShippingAddress, sa =>
        {
            sa.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("FirstName");
            sa.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("LastName");
            sa.Property(a => a.AddressLine1)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("AddressLine1");

            sa.Property(a => a.AddressLine2)
                .HasMaxLength(200)
                .HasColumnName("AddressLine2");

            sa.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("City");

            sa.Property(a => a.State)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("State");

            sa.Property(a => a.PostCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("PostCode");

            sa.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Country");
        });

        builder.HasMany(l => l.LineItems)
            .WithOne()
            .HasForeignKey("FulfillmentId")
            .HasPrincipalKey(f => f.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(f => f.LineItems)
            .HasField("_lineItems")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}