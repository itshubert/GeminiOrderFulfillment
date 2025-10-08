using ErrorOr;
using GeminiOrderFulfillment.Application.Common.Interfaces;
using GeminiOrderFulfillment.Application.Common.Models.Fulfillments;
using GeminiOrderFulfillment.Domain.Common.Errors;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate;
using MapsterMapper;
using MediatR;

namespace GeminiOrderFulfillment.Application.Fulfillments.Commands;

public sealed record CreateFulfillmentForOrderCommand(Guid OrderId) : IRequest<ErrorOr<FulfillmentModel?>>;

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

        var fulfillment = Fullfillment.Create(
            null,
            request.OrderId,
            Domain.FulfillmentAggregate.FulfillmentStatus.AWAITING_FULFILLMENT,
            null,
            DateTimeOffset.UtcNow,
            null);

        await _fulfillmentRepository.AddAsync(fulfillment, cancellationToken);

        return _mapper.Map<FulfillmentModel>(fulfillment);
    }
}


