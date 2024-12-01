using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Services;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class TestHttpController : ControllerBase
    {
        private readonly ITestHttpServiceTest _testHttpServiceTest;
        private readonly ITestHttpService _testHttpService2;
        private readonly ITestHttpService _testHttpService3;
        public TestHttpController(ITestHttpServiceTest testHttpServiceTest, 
            [FromKeyedServices("EXPLICIT_FACTORY_SCRATCH")] ITestHttpService testHttpService2, 
            [FromKeyedServices("EXPLICIT_FACTORY_CONFIG")] ITestHttpService testHttpService3)
        {
            _testHttpServiceTest = testHttpServiceTest;
            _testHttpService2 = testHttpService2;
            _testHttpService3 = testHttpService3;

        }

        [HttpGet("GetInfo")]
        public async Task<IResult> GetInfo()
        {
            await _testHttpServiceTest.CallHttp(HttpStatusCodeEnum.InternalServerError);

            return Results.NoContent();

        }

        [HttpGet("GetInfo2")]
        public async Task<IResult> GetInfo2()
        {
            await _testHttpService2.CallHttp(HttpStatusCodeEnum.InternalServerError);

            return Results.NoContent();

        }

        [HttpGet("GetInfo3")]
        public async Task<IResult> GetInfo3()
        {
            await _testHttpService3.CallHttp(HttpStatusCodeEnum.InternalServerError);

            return Results.NoContent();

        }


    }
}
