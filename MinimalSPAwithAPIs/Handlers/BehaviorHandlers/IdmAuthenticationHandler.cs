using Microsoft.Extensions.Logging;

namespace MinimalSPAwithAPIs.Handlers.BehaviorHandlers;

public class IdmAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "IDM";
}

public struct Roles
{
    public const string Role1 = "A1234:P00001";
}

public struct IdmClaimTypes
{
    public const string Name = "Mario";
    public const string Surname = "Rossi";
}

public class IdmAuthenticationHandler : AuthenticationHandler<IdmAuthenticationOptions>
{
    private const string MyHttpClaimName = "Mario";
    private const string MyHttpClaimSurname = "Rossi";
    private const string MyHttpClaimRoles = "My-RUOLI";

    private readonly IHttpContextAccessor accessor;
    private readonly List<string> allowedRoles = new();

    public IdmAuthenticationHandler
        (IHttpContextAccessor accessor,
        IConfiguration configuration,
        IOptionsMonitor<IdmAuthenticationOptions> options,
        ILoggerFactory logger, UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        this.accessor = accessor;
        allowedRoles = configuration.GetSection("Roles").Get<string[]>()?.ToList() ?? new List<string>();
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        await Task.Delay(0);

        var request = accessor.HttpContext!.Request;
        bool authorized = false;

        var name = request!.Headers[MyHttpClaimName];
        var surname = request!.Headers[MyHttpClaimSurname];
        var roles = request!.Headers[MyHttpClaimRoles];

        var myRoles = new List<string>();
        var userRoles = roles.ToString() ?? string.Empty;
        userRoles = userRoles.Trim().Replace(" ", string.Empty);
        foreach (var item in userRoles.Split('|'))
        {
            var parts = item.Split(',');
            var firstPart = parts.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(firstPart))
            {
                var role = firstPart.Split("=").LastOrDefault();
                if (!string.IsNullOrWhiteSpace(role))
                {
                    myRoles.Add(role);
                }
            }
        }
        myRoles = myRoles.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        if (!myRoles.Exists(x => allowedRoles.Exists(y => x.Contains(y))))
        {
            return AuthenticateResult.Fail($"Utente non autorizzato");
        }

        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, name.ToString() ?? string.Empty),
            new(ClaimTypes.Surname, surname.ToString() ?? string.Empty),
        };

        foreach (var role in myRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var claimsIdentity = new ClaimsIdentity
            (claims, Scheme.Name);

        var claimsPrincipal = new ClaimsPrincipal
            (claimsIdentity);

        authorized = true;

        return AuthenticateResult.Success
            (new AuthenticationTicket(claimsPrincipal,
            Scheme.Name));
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        await base.HandleChallengeAsync(properties);
        Response.Redirect("/Unauthorized");
    }
}