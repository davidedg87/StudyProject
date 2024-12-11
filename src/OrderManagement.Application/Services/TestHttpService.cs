using Microsoft.Extensions.Configuration;
using Nest;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;

namespace OrderManagement.Application.Services
{
    public enum HttpStatusCodeEnum
    {
        // Informational responses (100–199)
        Continue = 100,
        SwitchingProtocols = 101,
        Processing = 102,
        EarlyHints = 103,

        // Successful responses (200–299)
        OK = 200,
        Created = 201,
        Accepted = 202,
        NonAuthoritativeInformation = 203,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,

        // Redirection messages (300–399)
        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        SwitchProxy = 306,
        TemporaryRedirect = 307,
        PermanentRedirect = 308,

        // Client error responses (400–499)
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        PayloadTooLarge = 413,
        URITooLong = 414,
        UnsupportedMediaType = 415,
        RangeNotSatisfiable = 416,
        ExpectationFailed = 417,

            // Server error responses (500–599)
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HTTPVersionNotSupported = 505,
        VariantAlsoNegotiates = 506,
        InsufficientStorage = 507,
        LoopDetected = 508,
        NotExtended = 510,
        NetworkAuthenticationRequired = 511
    }

    public interface ITestHttpServiceTest
    {
        Task<string> CallHttp(HttpStatusCodeEnum httpStatusCodeEnum);
    }

    public interface ITestHttpService
    {
        Task<string> CallHttp(HttpStatusCodeEnum httpStatusCodeEnum);
    }

    //HttpClient configured in HttpExtensions directly to the service
    public class TestHttpService : ITestHttpServiceTest
    {
        private readonly HttpClient _httpClient;
        public TestHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<string> CallHttp(HttpStatusCodeEnum httpStatusCodeEnum)
        {
          int statusCode = (int)httpStatusCodeEnum;

          var response = await _httpClient.GetAsync($"status/{statusCode}");
          response.EnsureSuccessStatusCode();
          return await response.Content.ReadAsStringAsync();

        }

    }

    //Use of HttpClientFactory to create a client from scratch
    public class TestHttpService2 : ITestHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public TestHttpService2(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;

        }

        public async Task<string> CallHttp(HttpStatusCodeEnum httpStatusCodeEnum)
        {
            int statusCode = (int)httpStatusCodeEnum;

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress =  new Uri(_configuration["HttpTestUri"]!);

            var response = await client.GetAsync($"status/{statusCode}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();

        }

    }

    //Use of HttpClientFactory to retrieve e previously configured client in HttpExtensions
    public class TestHttpService3 : ITestHttpService
    {
        private readonly HttpClient httpClient;
        public TestHttpService3(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("TestHttpService3Client");

        }

        public async Task<string> CallHttp(HttpStatusCodeEnum httpStatusCodeEnum)
        {
            int statusCode = (int)httpStatusCodeEnum;

            var response = await httpClient.GetAsync($"status/{statusCode}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();

        }

    }

}
