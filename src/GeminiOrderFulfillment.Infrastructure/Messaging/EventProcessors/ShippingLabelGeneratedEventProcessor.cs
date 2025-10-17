using GeminiOrderFulfillment.Application.Common.Messaging;
using GeminiOrderFulfillment.Application.Common.Models.Fulfillments;
using GeminiOrderFulfillment.Application.Fulfillments.Commands;
using GeminiOrderFulfillment.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderFulfillment.Infrastructure.Messaging.EventProcessors;

public sealed class ShippingLabelGeneratedEventProcessor : IEventProcessor<ShippingLabelGeneratedEvent>
{
    private readonly IEventBridgePublisher _eventBridgePublisher;
    private readonly IMediator _mediator;
    private readonly ILogger<ShippingLabelGeneratedEventProcessor> _logger;

    public ShippingLabelGeneratedEventProcessor(
        IEventBridgePublisher eventBridgePublisher,
        IMediator mediator,
        ILogger<ShippingLabelGeneratedEventProcessor> logger)
    {
        _eventBridgePublisher = eventBridgePublisher;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<bool> ProcessEventAsync(ShippingLabelGeneratedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing ShippingLabelGeneratedEvent for OrderId: {OrderId}", @event.OrderId);

        var result = await _mediator.Send(new UpdateFulfillmentStatusCommand(
            @event.OrderId,
            FulfillmentStatus.LABEL_GENERATED,
            @event.TrackingNumber
        ));

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error updating order status for OrderId {OrderId}: {Error}", @event.OrderId, error);
            }

            return false;
        }

        await _eventBridgePublisher.PublishAsync(DetailTypes.OrderReadyForShipment, new
        {
            @event.OrderId,
            @event.TrackingNumber
        }, cancellationToken);

        _logger.LogInformation("OrderId: {OrderId} shipped with TrackingNumber: {TrackingNumber}", @event.OrderId, @event.TrackingNumber);

        return true;
    }
}