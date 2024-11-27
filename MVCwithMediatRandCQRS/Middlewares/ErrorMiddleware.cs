namespace MVCwithMediatRandCQRS.Web.Middlewares;

public class ErrorMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ErrorMiddleware> _logger;

    public ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
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

            _logger.LogError(ex, "Errore: {ErrorMessage}", errorMessage);

            var isAjax = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                context.Response.ContentType = "application/json";
                var statusCode = innerMostException switch
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
                context.Response.StatusCode = statusCode;

                var response = new
                {
                    statusCode,
                    errorMessage,
                    detail = innerMostException.StackTrace
                };

                _logger.LogError(ex, errorMessage);

                var jsonResponse = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(jsonResponse);
            }
            else
            {
                var errorUrl = $"/Error?message={Uri.EscapeDataString(errorMessage)}";
                context.Response.Redirect(errorUrl);
            }
        }
    }
}