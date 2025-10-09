using GeminiOrderFulfillment.Domain.FulfillmentAggregate;
using GeminiOrderFulfillment.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace GeminiOrderFulfillment.Infrastructure.Persistence;

public sealed class GeminiOrderFulfillmentDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public GeminiOrderFulfillmentDbContext(
        DbContextOptions<GeminiOrderFulfillmentDbContext> options,
        PublishDomainEventsInterceptor publishDomainEventsInterceptor
    ) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<Fulfillment> Fulfillments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeminiOrderFulfillmentDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}