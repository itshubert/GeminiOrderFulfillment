using GeminiOrderFulfillment.Application.Common.Messaging;
using GeminiOrderFulfillment.Application.Fulfillments.Commands;
using GeminiOrderFulfillment.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderFulfillment.Infrastructure.Messaging.EventProcessors;

public sealed class JobInProgressEventProcessor : IEventProcessor<JobInProgressEvent>
{
    private readonly IMediator _mediator;
    private readonly IEventBridgePublisher _eventBridgePublisher;
    private readonly ILogger<JobInProgressEventProcessor> _logger;

    public JobInProgressEventProcessor(
        IMediator mediator,
        IEventBridgePublisher eventBridgePublisher,
        ILogger<JobInProgressEventProcessor> logger)
    {
        _mediator = mediator;
        _eventBridgePublisher = eventBridgePublisher;
        _logger = logger;
    }

    public async Task<bool> ProcessEventAsync(JobInProgressEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing JobInProgressEvent: {EventId}", @event.JobId);

        var result = await _mediator.Send(new UpdateFulfillmentStatusCommand(
            @event.OrderId,
            Application.Common.Models.Fulfillments.FulfillmentStatus.PICKING_IN_PROGRESS
        ));

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error updating order status for OrderId {OrderId}: {Error}", @event.OrderId, error);
            }

            return false;
        }

        await _eventBridgePublisher.PublishAsync(DetailTypes.OrderInProgress, @event, cancellationToken);

        _logger.LogInformation("Job {JobId} for Order {OrderId} is in progress.", @event.JobId, @event.OrderId);

        return true;

    }
}