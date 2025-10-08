using GeminiOrderFulfillment.Domain.FulfillmentAggregate;

namespace GeminiOrderFulfillment.Application.Common.Interfaces;

public interface IFulfillmentRepository : IRepository
{
    Task<Fullfillment?> GetByOrderForUpdateAsync(Guid orderId, CancellationToken cancellationToken);
    Task AddAsync(Fullfillment fulfillment, CancellationToken cancellationToken);
}