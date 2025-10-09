namespace GeminiOrderFulfillment.Application.Common.Models.Fulfillments;

public sealed record ShippingAddressModel(
    string FirstName,
    string LastName,
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string PostCode,
    string Country
);