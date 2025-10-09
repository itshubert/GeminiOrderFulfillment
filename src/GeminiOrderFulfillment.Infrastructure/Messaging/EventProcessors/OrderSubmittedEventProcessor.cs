using GeminiOrderFulfillment.Application.Common.Messaging;
using GeminiOrderFulfillment.Application.Common.Models.Fulfillments;
using GeminiOrderFulfillment.Application.Fulfillments.Commands;
using GeminiOrderFulfillment.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderFulfillment.Infrastructure.Messaging.EventProcessors;

public sealed class OrderSubmittedEventProcessor : IEventProcessor<OrderSubmittedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderSubmittedEventProcessor> _logger;

    public OrderSubmittedEventProcessor(IMediator mediator, ILogger<OrderSubmittedEventProcessor> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<bool> ProcessEventAsync(OrderSubmittedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing OrderSubmittedEvent: {EventId}", @event.Id);

        var command = new CreateFulfillmentForOrderCommand(
            @event.Id,
            FulfillmentStatus.AWAITING_FULFILLMENT,
            new ShippingAddressModel(
                @event.ShippingAddress.FirstName,
                @event.ShippingAddress.LastName,
                @event.ShippingAddress.AddressLine1,
                @event.ShippingAddress.AddressLine2,
                @event.ShippingAddress.City,
                @event.ShippingAddress.State,
                @event.ShippingAddress.PostCode,
                @event.ShippingAddress.Country),
            @event.Items.Select(item => new FulfillmentLineItem(
                item.ProductId,
                item.Quantity,
                item.ProductName)));

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error creating fulfillment for order {OrderId}: {Error}", @event.Id, error.Description);
            }

            return false;
        }

        _logger.LogInformation("Successfully created fulfillment for order {OrderId}", @event.Id);

        return true;
    }
}