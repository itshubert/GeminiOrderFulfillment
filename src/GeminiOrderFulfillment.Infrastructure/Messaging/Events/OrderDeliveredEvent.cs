namespace GeminiOrderFulfillment.Infrastructure.Messaging.Events;

public sealed record OrderDeliveredEvent(Guid OrderId, string TrackingNumber, string Carrier);