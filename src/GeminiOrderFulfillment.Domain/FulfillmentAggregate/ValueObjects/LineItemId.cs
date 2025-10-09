using GeminiOrderFulfillment.Domain.Common.Models;

namespace GeminiOrderFulfillment.Domain.FulfillmentAggregate.ValueObjects;

public sealed class LineItemId : ValueObject
{
    public Guid Value { get; }

    private LineItemId(Guid value)
    {
        Value = value;
    }

    public static LineItemId Create(Guid value)
    {
        return new LineItemId(value);
    }

    public static LineItemId CreateUnique()
    {
        return new LineItemId(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}