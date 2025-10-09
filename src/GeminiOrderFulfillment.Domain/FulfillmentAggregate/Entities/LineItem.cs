using GeminiOrderFulfillment.Domain.Common.Models;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;

namespace GeminiOrderFulfillment.Domain.FulfillmentAggregate.Entities;

public sealed class LineItem : Entity<LineItemId>
{
    public FulfillmentId FulfillmentId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }

    private LineItem(
        LineItemId id,
        FulfillmentId fulfillmentId,
        Guid productId,
        string productName,
        int quantity) : base(id)
    {
        FulfillmentId = fulfillmentId;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
    }

#pragma warning disable CS8618
    private LineItem() : base(null!) { }
#pragma warning restore CS8618

    public static LineItem Create(
        Guid? lineItemId,
        FulfillmentId fulfillmentId,
        Guid productId,
        string productName,
        int quantity)
    {
        return new LineItem(
            lineItemId is null ? LineItemId.CreateUnique() : LineItemId.Create(lineItemId.Value),
            fulfillmentId,
            productId,
            productName,
            quantity);
    }
}