using GeminiOrderFulfillment.Domain.FulfillmentAggregate;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;

namespace GeminiOrderFulfillment.Application.Common.Interfaces;

public interface IFulfillmentRepository : IRepository
{
    Task<Fulfillment?> GetByIdAsync(FulfillmentId id, CancellationToken cancellationToken);
    Task<Fulfillment?> GetByOrderForUpdateAsync(Guid orderId, CancellationToken cancellationToken);
    Task AddAsync(Fulfillment fulfillment, CancellationToken cancellationToken);
}