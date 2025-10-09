namespace GeminiOrderFulfillment.Application.Common.Models.Fulfillments;

public sealed record FulfillmentTaskCreatedIntegrationModel(
    Guid FulfillmentId,
    Guid OrderId,
    string Status,
    string? TrackingNumber,
    ShippingAddressIntegrationModel ShippingAddress,
    List<LineItemIntegrationModel> LineItems,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);

public sealed record ShippingAddressIntegrationModel(
    string FirstName,
    string LastName,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostCode,
    string Country);

public sealed record LineItemIntegrationModel(
    Guid LineItemId,
    Guid ProductId,
    string ProductName,
    int Quantity);