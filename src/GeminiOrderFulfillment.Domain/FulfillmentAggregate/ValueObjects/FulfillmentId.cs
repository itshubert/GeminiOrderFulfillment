using GeminiOrderFulfillment.Domain.Common.Models;

namespace GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;

public sealed class FulfillmentId : ValueObject
{
    public Guid Value { get; }

    private FulfillmentId(Guid value)
    {
        Value = value;
    }

    public static FulfillmentId CreateUnique() => new(Guid.NewGuid());

    public static FulfillmentId Create(Guid value)
    {
        return new FulfillmentId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}