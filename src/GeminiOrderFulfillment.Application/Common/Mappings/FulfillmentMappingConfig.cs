using GeminiOrderFulfillment.Application.Common.Models.Fulfillments;
using GeminiOrderFulfillment.Domain.FulfillmentAggregate;
using Mapster;

namespace GeminiOrderFulfillment.Application.Common.Mappings;

public sealed class FulfillmentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Fulfillment, FulfillmentModel>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Status, src => (Models.Fulfillments.FulfillmentStatus)src.Status)
            .Map(dest => dest, src => src);

        config.NewConfig<Fulfillment, FulfillmentTaskCreatedIntegrationModel>()
            .Map(dest => dest.FulfillmentId, src => src.Id.Value)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.ShippingAddress, src => src.ShippingAddress.Adapt<ShippingAddressIntegrationModel>())
            .Map(dest => dest.LineItems, src => src.LineItems)
            .Map(dest => dest, src => src);

        config.NewConfig<Domain.FulfillmentAggregate.ValueObjects.ShippingAddress, ShippingAddressIntegrationModel>()
            .Map(dest => dest, src => src);

        config.NewConfig<Application.Common.Models.Fulfillments.FulfillmentStatus, Domain.FulfillmentAggregate.FulfillmentStatus>()
            .MapWith(src => Enum.Parse<Domain.FulfillmentAggregate.FulfillmentStatus>(src.ToString()));
    }
}