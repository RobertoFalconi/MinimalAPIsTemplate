namespace MVCwithMediatRandCQRS.Web.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
    public IActionResult Index(string message)
    {
        var errorMessage = message ?? "Errore sconosciuto.";
        ViewData["ErrorMessage"] = errorMessage;
        return View();
    }
}
