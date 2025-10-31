using ErrorOr;
using GeminiOrderFulfillment.Application.Common.Interfaces;
using GeminiOrderFulfillment.Application.Common.Models.Fulfillments;
using GeminiOrderFulfillment.Domain.Common.Errors;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.Entities;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;
using MapsterMapper;
using MediatR;

namespace GeminiOrderFulfillment.Application.Fulfillments.Commands;

public sealed record CreateFulfillmentForOrderCommand(
    Guid OrderId,
    Common.Models.Fulfillments.FulfillmentStatus Status,
    ShippingAddressModel ShippingAddress,
    IEnumerable<FulfillmentLineItem> LineItems) : IRequest<ErrorOr<FulfillmentModel?>>;

public sealed record FulfillmentLineItem(
    Guid ProductId,
    int Quantity,
    string ProductName);

public sealed class CreateFulfillmentForOrderCommandHandler : IRequestHandler<CreateFulfillmentForOrderCommand, ErrorOr<FulfillmentModel?>>
{
    private readonly IFulfillmentRepository _fulfillmentRepository;
    private readonly IMapper _mapper;

    public CreateFulfillmentForOrderCommandHandler(IFulfillmentRepository fulfillmentRepository, IMapper mapper)
    {
        _fulfillmentRepository = fulfillmentRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<FulfillmentModel?>> Handle(CreateFulfillmentForOrderCommand request, CancellationToken cancellationToken)
    {
        var conflictedFulfillment = await _fulfillmentRepository.GetByOrderForUpdateAsync(request.OrderId, cancellationToken);
        if (conflictedFulfillment is not null)
        {
            return Errors.Fulfillment.FulfillmentAlreadyExists(request.OrderId);
        }

        var fulfillmentId = FulfillmentId.CreateUnique();

        List<LineItem> lineItems = new();

        foreach (var item in request.LineItems)
        {
            var lineItem = LineItem.Create(
                null,
                fulfillmentId,
                item.ProductId,
                item.ProductName,
                item.Quantity);

            lineItems.Add(lineItem);
        }

        var fulfillment = Fulfillment.CreateWithItems(
            fulfillmentId.Value,
            request.OrderId,
            (Domain.FulfillmentAggregate.FulfillmentStatus)Common.Models.Fulfillments.FulfillmentStatus.AWAITING_FULFILLMENT,
            null,
            ShippingAddress.Create(
                request.ShippingAddress.FirstName,
                request.ShippingAddress.LastName,
                request.ShippingAddress.AddressLine1,
                request.ShippingAddress.AddressLine2,
                request.ShippingAddress.City,
                request.ShippingAddress.State,
                request.ShippingAddress.PostCode,
                request.ShippingAddress.Country),
            DateTimeOffset.UtcNow,
            null,
            lineItems);



        await _fulfillmentRepository.AddAsync(fulfillment, cancellationToken);

        fulfillment.UpdateStatus(Domain.FulfillmentAggregate.FulfillmentStatus.TASK_CREATED);

        await _fulfillmentRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FulfillmentModel>(fulfillment);
    }
}


