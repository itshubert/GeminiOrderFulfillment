using GeminiOrderFulfillment.Application.Common.Models.Fulfillments;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate;
using Mapster;

namespace GeminiOrderFulfillment.Application.Common.Mappings;

public sealed class FulfillmentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Fullfillment, FulfillmentModel>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Status, src => (Models.Fulfillments.FulfillmentStatus)src.Status)
            .Map(dest => dest, src => src);

    }
}