using E_Commerce.Application.DTOs.Response;
using System.Text.Json;

namespace E_Commerce.API.MiddleWare
{
    public class ExceptioneMiddelWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptioneMiddelWare> _logger;
        private readonly IHostEnvironment _env;
        public ExceptioneMiddelWare(RequestDelegate Next,ILogger<ExceptioneMiddelWare> logger,IHostEnvironment env)
        {
            _next = Next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode =  500;

                var Response = _env.IsDevelopment() ? new ApiExceptionResponse(false,500, "Internal Server Error", ex.Message) : new ApiExceptionResponse(false,500,"Internal Error Server");
                var Option = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var JsonRespose = JsonSerializer.Serialize(Response, Option);
                await context.Response.WriteAsync(JsonRespose);
            }
        }
    }
}
