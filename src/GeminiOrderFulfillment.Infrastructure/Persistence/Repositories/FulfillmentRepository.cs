using GeminiOrderFulfillment.Application.Common.Interfaces;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate;
using Microsoft.EntityFrameworkCore;

namespace GeminiOrderFulfillment.Infrastructure.Persistence.Repositories;

public sealed class FulfillmentRepository : BaseRepository, IFulfillmentRepository
{
    public FulfillmentRepository(GeminiOrderFulfillmentDbContext context) : base(context)
    {
    }

    public async Task<Fullfillment?> GetByOrderForUpdateAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await _context.Fulfillments
            .Where(f => f.OrderId == orderId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Fullfillment fulfillment, CancellationToken cancellationToken)
    {
        await _context.Fulfillments.AddAsync(fulfillment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}