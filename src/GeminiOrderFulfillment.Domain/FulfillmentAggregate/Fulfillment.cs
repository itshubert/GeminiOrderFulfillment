using GeminiOrderFulfillment.Domain.Common.Models;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.Entities;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.Events;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;

namespace GeminiOrderFulfillment.Domain.FulfillmentAggregate;

public sealed class Fulfillment : AggregateRoot<FulfillmentId>
{
    private readonly List<LineItem> _lineItems = new();

    public Guid OrderId { get; private set; }
    public FulfillmentStatus Status { get; private set; }
    public string? TrackingNumber { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public ShippingAddress ShippingAddress { get; private set; }
    public IReadOnlyList<LineItem> LineItems => _lineItems.AsReadOnly();

    private Fulfillment(
        FulfillmentId id,
        Guid orderId,
        FulfillmentStatus status,
        string? trackingNumber,
        ShippingAddress shippingAddress,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt) : base(id)
    {
        OrderId = orderId;
        Status = status;
        TrackingNumber = trackingNumber;
        ShippingAddress = shippingAddress;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

#pragma warning disable CS8618
    private Fulfillment() : base(null!) { }
#pragma warning restore CS8618

    public static Fulfillment Create(
        Guid? fullfillmentId,
        Guid orderId,
        FulfillmentStatus status,
        string? trackingNumber,
        ShippingAddress shippingAddress,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt)
    {
        var fulfillment = new Fulfillment(
            fullfillmentId is null ? FulfillmentId.CreateUnique() : FulfillmentId.Create(fullfillmentId.Value),
            orderId,
            status,
            trackingNumber,
            shippingAddress,
            createdAt,
            updatedAt);

        fulfillment.AddDomainEvent(new FulfillmentCreatedDomainEvent(fulfillment.Id));

        return fulfillment;
    }

    public static Fulfillment CreateWithItems(
        Guid? fullfillmentId,
        Guid orderId,
        FulfillmentStatus status,
        string? trackingNumber,
        ShippingAddress shippingAddress,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt,
        IEnumerable<LineItem> lineItems)
    {
        var fulfillment = new Fulfillment(
            fullfillmentId is null ? FulfillmentId.CreateUnique() : FulfillmentId.Create(fullfillmentId.Value),
            orderId,
            status,
            trackingNumber,
            shippingAddress,
            createdAt,
            updatedAt);

        foreach (var item in lineItems)
        {
            fulfillment._lineItems.Add(item);
        }

        if (status == FulfillmentStatus.TASK_CREATED)
        {
            fulfillment.AddDomainEvent(new FulfillmentCreatedDomainEvent(fulfillment.Id));
        }

        return fulfillment;
    }

    public void AddLineItem(LineItem lineItem)
    {
        _lineItems.Add(lineItem);
    }

    public void UpdateStatus(FulfillmentStatus status)
    {
        Status = status;

        if (status == FulfillmentStatus.TASK_CREATED)
        {
            AddDomainEvent(new FulfillmentCreatedDomainEvent(Id));
        }
    }

    public void UpdateTrackingNumber(string trackingNumber)
    {
        TrackingNumber = trackingNumber;
    }

}