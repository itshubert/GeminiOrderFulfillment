using ErrorOr;
using GeminiOrderFulfillment.Application.Common.Interfaces;
using GeminiOrderFulfillment.Domain.Common.Errors;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate;
using MediatR;

namespace GeminiOrderFulfillment.Application.Fulfillments.Commands;

public sealed record UpdateFulfillmentStatusCommand(
    Guid OrderId,
    Application.Common.Models.Fulfillments.FulfillmentStatus NewStatus,
    string? TrackingNumber) : IRequest<ErrorOr<Success>>;

public sealed class UpdateFulfillmentStatusCommandHandler(IFulfillmentRepository _fulfillmentRepository)
    : IRequestHandler<UpdateFulfillmentStatusCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(UpdateFulfillmentStatusCommand request, CancellationToken cancellationToken)
    {
        var fulfillment = await _fulfillmentRepository.GetByOrderForUpdateAsync(request.OrderId, cancellationToken);

        if (fulfillment is null)
        {
            return Errors.Fulfillment.InvalidOrderId(request.OrderId);
        }

        fulfillment.UpdateStatus((FulfillmentStatus)request.NewStatus);

        if (!string.IsNullOrEmpty(request.TrackingNumber))
        {
            fulfillment.UpdateTrackingNumber(request.TrackingNumber);
        }

        await _fulfillmentRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}