using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace MinimalSPAwithAPIs.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class ErrorModel : PageModel
{
    private readonly ILogger<ErrorModel> _logger;

    private readonly HttpContext _httpContext;

    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    public ErrorModel(ILogger<ErrorModel> logger, HttpContext httpContext)
    {
        _logger = logger;
        _httpContext = httpContext;
    }

    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? _httpContext.TraceIdentifier;
    }
}
