using ErrorOr;
using GeminiOrderFulfillment.Application.Common.Interfaces;
using GeminiOrderFulfillment.Application.Common.Messaging;
using GeminiOrderFulfillment.Application.Common.Models.Fulfillments;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderFulfillment.Application.Fulfillments.Commands;

public sealed record PublishFulfillmentsByStatusCommand(FulfillmentStatus Status) : IRequest<ErrorOr<Success>>;

public sealed class PublishFulfillmentsByStatusCommandHandler : IRequestHandler<PublishFulfillmentsByStatusCommand, ErrorOr<Success>>
{
    private readonly IFulfillmentRepository _fulfillmentRepository;
    private readonly IEventBridgePublisher _eventBridgePublisher;
    private readonly IMapper _mapper;
    private readonly ILogger<PublishFulfillmentsByStatusCommandHandler> _logger;

    public PublishFulfillmentsByStatusCommandHandler(
        IFulfillmentRepository fulfillmentRepository,
        IEventBridgePublisher eventBridgePublisher,
        IMapper mapper,
        ILogger<PublishFulfillmentsByStatusCommandHandler> logger)
    {
        _fulfillmentRepository = fulfillmentRepository;
        _eventBridgePublisher = eventBridgePublisher;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ErrorOr<Success>> Handle(PublishFulfillmentsByStatusCommand request, CancellationToken cancellationToken)
    {
        var fulfillments = await _fulfillmentRepository.GetByStatusAsync(_mapper.Map<Domain.FulfillmentAggregate.FulfillmentStatus>(request.Status), cancellationToken);

        foreach (var fulfillment in fulfillments)
        {
            var fulfillmentModel = _mapper.Map<FulfillmentModel>(fulfillment);

            _logger.LogInformation("Publishing fulfillment with ID: {FulfillmentId} and Status: {Status}", fulfillmentModel.Id, fulfillmentModel.Status);

            DetailTypes detailType = request.Status switch
            {
                FulfillmentStatus.TASK_CREATED => DetailTypes.FulfillmentTaskCreated,
                FulfillmentStatus.PICKING_IN_PROGRESS => DetailTypes.OrderInProgress,
                FulfillmentStatus.PACKED => DetailTypes.ShippingJobCreated,
                FulfillmentStatus.LABEL_GENERATED => DetailTypes.OrderReadyForShipment,
                FulfillmentStatus.ORDER_SHIPPED => DetailTypes.OrderShipped,
                _ => throw new InvalidOperationException($"No detail type mapping for fulfillment status: {request.Status}")
            };

            await _eventBridgePublisher.PublishAsync(detailType, new { fulfillment.OrderId, fulfillment.TrackingNumber }, cancellationToken);
        }

        return Result.Success;
    }
}