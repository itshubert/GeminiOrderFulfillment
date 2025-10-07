namespace GeminiOrderFulfillment.Infrastructure.Messaging;

public sealed class QueueSettings
{
    public string InventoryReserved { get; set; } = string.Empty;
    public string ReadyForPicking { get; set; } = string.Empty;
}