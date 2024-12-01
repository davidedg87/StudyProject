using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Infrastructure.Data.DbContext;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class OrdersItemsController : ControllerBase
    {
    
        [HttpGet("orderId")] 
        public async Task<IResult> GetOrderCount(int orderId, [FromServices] AppDbContext appDbContext)
        {
            var ordersAsync = await appDbContext.GetAllData_Compiled_Async(orderId);
            return Results.Ok(ordersAsync);
        }

    

    }

}

