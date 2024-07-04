namespace MinimalSPAwithAPIs.Handlers.BehaviorHandlers;

public class ErrorHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandler> _logger;

    public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var innerMostException = ex;

            var errorMessage = ex.Message;

            while (innerMostException.InnerException != null)
            {
                if (!innerMostException.Message.Contains(innerMostException.InnerException.Message))
                {
                    errorMessage += $" {innerMostException.InnerException.Message}";
                }
                innerMostException = innerMostException.InnerException;
            }

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex.InnerException switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                DbUpdateException => (int)HttpStatusCode.Conflict,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                NotImplementedException => (int)HttpStatusCode.NotImplemented,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                TimeoutException => (int)HttpStatusCode.RequestTimeout,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            var response = new
            {
                message = errorMessage,
                detail = innerMostException.StackTrace
            };

            _logger.LogError(ex, errorMessage);

            var jsonResponse = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}