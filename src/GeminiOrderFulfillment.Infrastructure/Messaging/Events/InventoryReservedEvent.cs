namespace GeminiOrderFulfillment.Infrastructure.Messaging.Events;

public sealed record InventoryReserved(
    Guid OrderId,
    IEnumerable<InventoryReservedItem> Items);

public sealed record InventoryReservedItem(
    Guid ProductId,
    int Quantity,
    string ProductName);