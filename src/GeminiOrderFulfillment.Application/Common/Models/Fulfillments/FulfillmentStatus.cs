namespace GeminiOrderFulfillment.Application.Common.Models.Fulfillments;

public enum FulfillmentStatus
{
    AWAITING_FULFILLMENT,
    TASK_CREATED,
    PICKING_IN_PROGRESS,
    PACKED,
    LABEL_GENERATED,
    ORDER_SHIPPED,
    IN_TRANSIT,
    DELIVERED
}