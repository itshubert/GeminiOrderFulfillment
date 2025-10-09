using GeminiOrderFulfillment.Application.Common.Interfaces;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GeminiOrderFulfillment.Infrastructure.Persistence.Repositories;

public sealed class FulfillmentRepository : BaseRepository, IFulfillmentRepository
{
    public FulfillmentRepository(GeminiOrderFulfillmentDbContext context) : base(context)
    {
    }

    public async Task<Fulfillment?> GetByIdAsync(FulfillmentId id, CancellationToken cancellationToken)
    {
        return await _context.Fulfillments
            .Include(f => f.LineItems)
            .Where(f => f.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Fulfillment?> GetByOrderForUpdateAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await _context.Fulfillments
            .Where(f => f.OrderId == orderId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Fulfillment fulfillment, CancellationToken cancellationToken)
    {
        await _context.Fulfillments.AddAsync(fulfillment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}