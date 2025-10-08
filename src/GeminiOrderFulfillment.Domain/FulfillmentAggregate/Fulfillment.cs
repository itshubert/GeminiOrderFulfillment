using GeminiOrderFulfillment.Domain.Common.Models;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;

namespace GeminiOrderFulfillment.Domain.FulfillmentAggregate;

public sealed class Fullfillment : AggregateRoot<FulfillmentId>
{
    public Guid OrderId { get; private set; }
    public FulfillmentStatus Status { get; private set; }
    public string? TrackingNumber { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private Fullfillment(
        FulfillmentId id,
        Guid orderId,
        FulfillmentStatus status,
        string? trackingNumber,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt) : base(id)
    {
        OrderId = orderId;
        Status = status;
        TrackingNumber = trackingNumber;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

#pragma warning disable CS8618
    private Fullfillment() : base(null!) { }
#pragma warning restore CS8618

    public static Fullfillment Create(
        Guid? fullfillmentId,
        Guid orderId,
        FulfillmentStatus status,
        string? trackingNumber,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt)
    {
        return new Fullfillment(
            fullfillmentId is null ? FulfillmentId.CreateUnique() : FulfillmentId.Create(fullfillmentId.Value),
            orderId,
            status,
            trackingNumber,
            createdAt,
            updatedAt);
    }

    public void UpdateStatus(FulfillmentStatus status)
    {
        Status = status;
    }

    public void UpdateTrackingNumber(string trackingNumber)
    {
        TrackingNumber = trackingNumber;
    }

}