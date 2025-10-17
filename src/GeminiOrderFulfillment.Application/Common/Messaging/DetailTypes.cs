namespace GeminiOrderFulfillment.Application.Common.Messaging;

public enum DetailTypes
{
    FulfillmentTaskCreated,
    ShippingJobCreated,
    OrderInProgress,
    OrderReadyForShipment,
    OrderShipped
}

// TODO: Publish OrderShipped when Carrier publishes shipment event