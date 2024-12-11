using Asp.Versioning;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Services;
using OrderManagement.Core.Interfaces;
using Order = OrderManagement.Core.Entities.Order;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class OrdersController : ControllerBase
    {
        #region Different way for GetOrderById

        [HttpGet("by-route-param/{id}")] //Param defined in route parameter
       // [ApiKeyRequired]
        public async Task<IResult> GetOrderById(int id, [FromServices] IOrderRepository orderRepository, [FromServices] IRepository<Order> repository)
        {
            var order = await orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return Results.NotFound("Ordine non trovato.");
            }

            return Results.Ok(order);

        }

        [HttpGet("by-query")] //Param defined in query string
        public async Task<IResult> GetOrderByIdFromQuery([FromQuery] int id, [FromServices] IOrderRepository orderRepository)
        {
            var order = await orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return Results.NotFound("Ordine non trovato.");
            }

            return Results.Ok(order);
        }

        [HttpGet("by-header")] //Param defined in specific header
        public async Task<IResult> GetOrderByIdFromHeader([FromHeader(Name = "X-Order-Id")] int id, [FromServices] IOrderRepository orderRepository)
        {
            var order = await orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return Results.NotFound("Ordine non trovato.");
            }

            return Results.Ok(order);
        }

        [HttpPost("by-body")] //Param defined in body ( works with endpoint that has a payload like POST , PUT, PATCH)
        public async Task<IResult> GetOrderByIdFromBody([FromBody] int id, [FromServices] IOrderRepository orderRepository, [FromServices] NotifierService notifierService)
        {
            var order = await orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return Results.NotFound("Ordine non trovato.");
            }

            notifierService.Notify("GetOrderByIdFromBody");

            return Results.Ok(order);
        }

        [HttpPost("by-form")] //Param passed in formdata
        public async Task<IResult> GetOrderByIdFromForm([FromForm] int id, [FromServices] IOrderRepository orderRepository)
        {
            var order = await orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return Results.NotFound("Ordine non trovato.");
            }

            return Results.Ok(order);
        }

        #endregion

        [HttpGet("count")] //Route definition
        //[ApiKeyRequired]
        public async Task<IResult> GetOrderCount([FromServices] IOrderRepository orderRepository)
        {
            var order = await orderRepository.GetOrdersCount();

            return Results.Ok(order.Data);
        }

        [HttpPost]
        public async Task<IResult> CreateOrder([FromServices] IBus bus,   [FromBody] Order order, [FromServices] IOrderRepository orderRepository)
        {
            if (order == null)
            {
                return Results.BadRequest("L'ordine non può essere nullo.");
            }

            await orderRepository.AddAsync(order);

            return Results.NoContent();
        }


        [HttpPut]
        public async Task<IResult> UpdateOrder([FromBody] Order order, [FromServices] IOrderRepository orderRepository)
        {
            var existingOrder = await orderRepository.Query().AnyAsync(x => x.Id == order.Id);
            if (!existingOrder)
            {
                return Results.NotFound("Ordine non trovato.");
            }

            // Aggiorna le proprietà dell'ordine
            await orderRepository.UpdateAsync(order);

            return Results.NoContent(); // Restituisce 204 No Content per un aggiornamento riuscito
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteOrder(int id, [FromServices] IOrderRepository orderRepository)
        {
            var existingOrder = await orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return Results.NotFound("Ordine non trovato.");
            }

            await orderRepository.DeleteAsync(id);

            return Results.NoContent(); // Restituisce 204 No Content per una cancellazione riuscita
        }


        [HttpDelete("DeleteBulk")]
        public async Task<IResult> DeleteBulkOrders([FromServices] IOrderRepository orderRepository, [FromBody] List<int> ids)
        {
            await orderRepository.DeleteBulkAsync(ids);
            return Results.NoContent(); // Restituisce 204 No Content per una cancellazione riuscita
        }


        [HttpDelete("SoftDeleteBulk")]
        public async Task<IResult> SoftDeleteBulkOrders([FromServices] IOrderRepository orderRepository, [FromBody] List<int> ids)
        {
            await orderRepository.SoftDeleteBulkAsync(ids);
            return Results.NoContent(); // Restituisce 204 No Content per una cancellazione riuscita
        }

    }

}

