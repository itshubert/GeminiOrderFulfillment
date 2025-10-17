using GeminiOrderFulfillment.Application.Common.Messaging;
using GeminiOrderFulfillment.Application.Common.Models.Fulfillments;
using GeminiOrderFulfillment.Application.Fulfillments.Commands;
using GeminiOrderFulfillment.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderFulfillment.Infrastructure.Messaging.EventProcessors;

public sealed class OrderDeliveredEventProcessor : IEventProcessor<OrderDeliveredEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderDeliveredEventProcessor> _logger;

    public OrderDeliveredEventProcessor(
        IMediator mediator,
        ILogger<OrderDeliveredEventProcessor> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<bool> ProcessEventAsync(OrderDeliveredEvent eventData, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing OrderDeliveredEvent for OrderId: {OrderId}, TrackingNumber: {TrackingNumber}, Carrier: {Carrier}",
            eventData.OrderId, eventData.TrackingNumber, eventData.Carrier);


        var result = await _mediator.Send(new UpdateFulfillmentStatusCommand(
            eventData.OrderId,
            FulfillmentStatus.DELIVERED,
            eventData.TrackingNumber), cancellationToken);

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error updating order status for OrderId {OrderId}: {Error}", eventData.OrderId, error);
            }

            return false;
        }

        return true;
    }
}