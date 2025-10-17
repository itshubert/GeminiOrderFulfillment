namespace GeminiOrderFulfillment.Application.Common.Messaging;

public enum DetailTypes
{
    FulfillmentTaskCreated,
    ShippingJobCreated,
    OrderInProgress,
    OrderReadyForShipment,
    OrderShipped
}
