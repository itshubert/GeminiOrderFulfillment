using GeminiOrderFulfillment.Application.Common.Messaging;
using GeminiOrderFulfillment.Application.Fulfillments.Commands;
using GeminiOrderFulfillment.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderFulfillment.Infrastructure.Messaging.EventProcessors;

public sealed class JobCompletedEventProcessor : IEventProcessor<JobCompletedEvent>
{
    private readonly IEventBridgePublisher _eventBridgePublisher;
    private readonly IMediator _mediator;
    private readonly ILogger<JobCompletedEventProcessor> _logger;

    public JobCompletedEventProcessor(
        IEventBridgePublisher eventBridgePublisher,
        IMediator mediator,
        ILogger<JobCompletedEventProcessor> logger)
    {
        _eventBridgePublisher = eventBridgePublisher;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<bool> ProcessEventAsync(JobCompletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing JobCompletedEvent: {EventId}", @event.JobId);

        var result = await _mediator.Send(new UpdateFulfillmentStatusCommand(
            @event.OrderId,
            Application.Common.Models.Fulfillments.FulfillmentStatus.PACKED
        ));

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error updating order status for OrderId {OrderId}: {Error}", @event.OrderId, error);
            }

            return false;
        }

        await _eventBridgePublisher.PublishAsync(DetailTypes.ShippingJobCreated, new
        {
            @event.OrderId
        }, cancellationToken);

        _logger.LogInformation("Job {JobId} for Order {OrderId} completed at {CompletedAt}.", @event.JobId, @event.OrderId, @event.CompletedAt);

        return true;

    }
}