using ErrorOr;
using GeminiOrderFulfillment.Application.Common.Interfaces;
using GeminiOrderFulfillment.Application.Common.Messaging;
using GeminiOrderFulfillment.Application.Common.Models.Fulfillments;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.Events;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderFulfillment.Application.Fulfillments.EventHandlers;

public sealed record FulfillmentCreatedDomainEventHandler : INotificationHandler<FulfillmentCreatedDomainEvent>
{
    private readonly IFulfillmentRepository _fulfillmentRepository;
    private readonly IEventBridgePublisher _eventBridgePublisher;
    private readonly IMapper _mapper;
    private readonly ILogger<FulfillmentCreatedDomainEventHandler> _logger;

    public FulfillmentCreatedDomainEventHandler(
        IFulfillmentRepository fulfillmentRepository,
        IEventBridgePublisher eventBridgePublisher,
        IMapper mapper,
        ILogger<FulfillmentCreatedDomainEventHandler> logger)
    {
        _fulfillmentRepository = fulfillmentRepository;
        _eventBridgePublisher = eventBridgePublisher;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(FulfillmentCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling FulfillmentCreatedDomainEvent for FulfillmentId: {FulfillmentId}", notification.FulfillmentId);

        var fulfillment = await _fulfillmentRepository.GetByIdAsync(notification.FulfillmentId, cancellationToken);

        if (fulfillment is null)
        {
            _logger.LogWarning("Fulfillment with Id: {FulfillmentId} not found.", notification.FulfillmentId);
            return;
        }

        var fulfillmentCreatedEvent = _mapper.Map<FulfillmentTaskCreatedIntegrationModel>(fulfillment);

        await _eventBridgePublisher.PublishAsync(DetailTypes.FulfillmentTaskCreated, fulfillmentCreatedEvent, cancellationToken);

        _logger.LogInformation("Published FulfillmentCreatedEvent for FulfillmentId: {FulfillmentId}", notification.FulfillmentId);


    }
}