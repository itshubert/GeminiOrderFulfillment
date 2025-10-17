using GeminiOrderFulfillment.Application.Fulfillments.Commands;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiOrderFulfillment.Api.Controllers;

[Route("[controller]")]
public class FulfillmentsController : BaseController
{
    public FulfillmentsController(ISender mediator, IMapper mapper)
        : base(mediator, mapper)
    {
    }

    [HttpPost("publish-by-status/{status}")]
    public async Task<IActionResult> PublishFulfillmentsByStatusAsync([FromRoute] string status, CancellationToken cancellationToken)
    {
        var command = new PublishFulfillmentsByStatusCommand(status.ToLower() switch
        {
            "task_created" => Application.Common.Models.Fulfillments.FulfillmentStatus.TASK_CREATED,
            "picking_in_progress" => Application.Common.Models.Fulfillments.FulfillmentStatus.PICKING_IN_PROGRESS,
            "packed" => Application.Common.Models.Fulfillments.FulfillmentStatus.PACKED,
            "label_generated" => Application.Common.Models.Fulfillments.FulfillmentStatus.LABEL_GENERATED,
            "order_shipped" => Application.Common.Models.Fulfillments.FulfillmentStatus.ORDER_SHIPPED,
            _ => throw new ArgumentException($"Invalid fulfillment status: {status}")
        });

        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            _ => Ok(),
            Problem
        );
    }

}