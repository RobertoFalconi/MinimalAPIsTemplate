namespace MVCwithMediatRandCQRS.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IMediator _mediator;

    public HomeController(ILogger<HomeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var vm = await _mediator.Send(new GetUtenteByIdQuery(1));

        return View(vm);
    }

    // C#
    [HttpPost]
    public async Task<IActionResult> CreateUser(string Nome, string Cognome, string Email)
    {
        if (string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Cognome) || string.IsNullOrWhiteSpace(Email))
        {
            return BadRequest("Tutti i campi sono obbligatori.");
        }

        var userId = await _mediator.Send(new CreateUtenteCommand(Nome, Cognome, Email));

        return Json(new { id = userId });
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
