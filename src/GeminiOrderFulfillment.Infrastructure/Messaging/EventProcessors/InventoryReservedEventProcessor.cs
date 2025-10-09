using GeminiOrderFulfillment.Application.Common.Messaging;
using GeminiOrderFulfillment.Application.Common.Models.Fulfillments;
using GeminiOrderFulfillment.Application.Fulfillments.Commands;
using GeminiOrderFulfillment.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderFulfillment.Infrastructure.Messaging.EventProcessors;

public sealed class InventoryReservedEventProcessor : IEventProcessor<InventoryReservedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<InventoryReservedEventProcessor> _logger;

    public InventoryReservedEventProcessor(IMediator mediator, ILogger<InventoryReservedEventProcessor> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<bool> ProcessEventAsync(InventoryReservedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing InventoryReservedEvent: {EventId}", @event.OrderId);

        var result = await _mediator.Send(new UpdateFulfillmentStatusCommand(
            @event.OrderId,
            FulfillmentStatus.TASK_CREATED
        ));

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error updating order status for OrderId {OrderId}: {Error}", @event.OrderId, error);
            }

            return false;
        }

        _logger.LogInformation("Successfully updated order status for OrderId {OrderId}", @event.OrderId);

        return true;
    }
}