namespace GeminiOrderFulfillment.Infrastructure.Messaging.Events;

public sealed record ShippingLabelGeneratedEvent(Guid OrderId, string TrackingNumber);