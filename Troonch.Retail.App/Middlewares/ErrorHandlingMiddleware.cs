using System.Text;

namespace Troonch.Retail.App.Middlewares;

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
            //var request = httpContext.Request;
            //if (request.Method == HttpMethods.Post && request.ContentLength > 0)
            //{
            //    request.EnableBuffering();
            //    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            //    await request.Body.ReadAsync(buffer, 0, buffer.Length);
            //    var requestContent = Encoding.UTF8.GetString(buffer);
                
            //    _logger.LogInformation($"{request.Path} - Body: {requestContent}");

            //    request.Body.Position = 0; 
            //}

            //var response = httpContext.Response;
            
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.Redirect($"/Error/{context.Response.StatusCode}/{exception.Message}");
        
        return Task.CompletedTask;
    }
}
