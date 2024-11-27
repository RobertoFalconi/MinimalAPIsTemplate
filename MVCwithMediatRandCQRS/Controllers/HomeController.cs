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
        var request = new UtenteServiceRequest();
        request.Id = 1;
        var utenteEntity = await _mediator.Send(new GetUtenteByIdQuery(request));

        var utenteViewModel = new UtenteViewModel();
        utenteViewModel.Nome = utenteEntity.Nome;

        return View(utenteViewModel);
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
