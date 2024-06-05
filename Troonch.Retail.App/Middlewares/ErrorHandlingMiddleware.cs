using System.Net;

namespace Troonch.Retail.App.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "text/html";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = "<html><body>\r\n" +
                         "An error occurred.<br>\r\n" +
                         "<a href=\"/\">Home</a><br>\r\n" +
                         $"<div>Exception: {exception.Message}</div>" +
                         "</body></html>\r\n";

            return context.Response.WriteAsync(result);
        }
    }
}
