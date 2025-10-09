using GeminiOrderFulfillment.Domain.Common.Models;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;

namespace GeminiOrderFulfillment.Domain.FulfillmentAggregate.Events;

public sealed record FulfillmentCreatedDomainEvent(FulfillmentId FulfillmentId) : IDomainEvent;