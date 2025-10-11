namespace GeminiOrderFulfillment.Infrastructure.Messaging.Events;

public sealed record JobCompletedEvent(
    Guid JobId,
    Guid OrderId,
    string JobType,
    DateTime CompletedAt);