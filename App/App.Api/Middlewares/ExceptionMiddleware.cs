using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace App.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError; // 500 if unexpected

            string message = "Sunucu hatası oluştu.";
            string detailed = _env.IsDevelopment() ? exception.ToString() : "Bir hata oluştu.";

            // Örnek: Özel exception türlerini burada yakalayabiliriz
            if (exception is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = "Yetkisiz erişim.";
            }
            // İstersen diğer özel exception tiplerini ekleyebilirsin

            _logger.LogError(exception, "Unhandled exception occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorDetails
            {
                StatusCode = (int)statusCode,
                Message = message,
                Detailed = detailed
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
        public class ErrorDetails
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public string Detailed { get; set; }
        }
    }


    
}

