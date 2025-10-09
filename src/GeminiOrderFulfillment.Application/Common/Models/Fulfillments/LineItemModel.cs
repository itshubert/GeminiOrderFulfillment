namespace GeminiOrderFulfillment.Application.Common.Models.Fulfillments;

public sealed record LineItemModel(
    Guid ProductId,
    string ProductName,
    int Quantity);