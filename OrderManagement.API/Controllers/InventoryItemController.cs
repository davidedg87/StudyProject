
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Commands;
using OrderManagement.Application.Queries;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InventoryItemController : ControllerBase
    {

        private readonly IMediator _mediator;

        public InventoryItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        public async Task<IActionResult> CreateV1([FromBody] CreateInventoryItemCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(CreateV1), new { id = result }, result);
        }

        [MapToApiVersion("2.0")]
        [HttpPost]
        public async Task<IActionResult> CreateV2([FromBody] CreateInventoryItemCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(CreateV2), new { id = result }, result);
        }


        [MapToApiVersion("1.0")]
        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] UpdateInventoryItemCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);

            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpPut("{id}/decrease")]
        public async Task<IActionResult> Decrease(int id, [FromBody] DecreaseInventoryItemCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);

            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

           var query = new GetInventoryItemQuery { Id = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }



    }
}
