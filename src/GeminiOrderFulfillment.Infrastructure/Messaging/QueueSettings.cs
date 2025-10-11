namespace GeminiOrderFulfillment.Infrastructure.Messaging;

public sealed class QueueSettings
{
    public string InventoryReserved { get; set; } = string.Empty;
    public string ReadyForPicking { get; set; } = string.Empty;
    public string OrderSubmitted { get; set; } = string.Empty;
    public string JobInProgress { get; set; } = string.Empty;
    public string JobCompleted { get; set; } = string.Empty;
    public string ShippingLabelGenerated { get; set; } = string.Empty;
}