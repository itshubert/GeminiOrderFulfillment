namespace GeminiOrderFulfillment.Application.Common.Models.Fulfillments;

public sealed record FulfillmentModel(
    Guid Id,
    Guid OrderId,
    FulfillmentStatus Status,
    string? TrackingNumber,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);