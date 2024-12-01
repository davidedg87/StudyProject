using Microsoft.AspNetCore.Mvc;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    public class ExaminationController : ControllerBase
    {
        public ExaminationController()
        {
            
        }


        [HttpGet("products")]
        public async Task<IResult> GetProducts()
        { 
            return Results.Ok("Products");

        }

        [HttpGet("prodcuts/{id}")]
        public async Task<IResult> GetProduct(int id)
        {
            return Results.Ok("Product");
        }

        [HttpPost("products")]
        public async Task<IResult> AddProduct([FromBody] List<string> producuts)
        {

            return Results.Ok("Product added");
        }

        [HttpPut("products/{id}")]
        public async Task<IResult> UpdateProduct(int id, [FromBody] string product)
        {
            return Results.Ok("Product updated");

        }

        [HttpDelete("products/{id}")]
        public async Task<IResult> DeleteProduct(int id)
        {
            return Results.Ok("Product deleted");
        }

    }
}
