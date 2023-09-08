using API.Data.DTOs;
using System.Net;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;

        private readonly IWebHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

            }catch(Exception ex)
            {
                if(_env.IsDevelopment())
                {
                    _logger.LogError(ex, $"{ex.Message}, User: {context.User.Identity?.Name}");
                }
                else
                {
                    _logger.LogError(ex, ex.Message);
                }

                await HandleExceptionAsync(context);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsync(new ExceptionDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "Internal server error"
            }.ToString());
        }
    }
}
