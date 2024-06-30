namespace MinimalSPAwithAPIs.Handlers.AuthenticationHandlers;

public class IdmAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "IDM";
}

public struct Roles
{
    public const string Profilo1 = "A1234:P00001";
    public const string Profilo2 = "A1234:P00002";
    public const string Profilo3 = "A1234:P00003";
    public const string Profilo4 = "A1234:P00004";
}

public struct IdmClaimTypes
{
    public const string CodiceFiscale = "CodiceFiscale";
    public const string Nominativo = "Nominativo";
    public const string CodiceSede = "CodiceSede";
    public const string CodiceSedeSap = "CodiceSedeSap";
    public const string ProfiloVega = "ProfiloVega";
    public const string Qualifica = "Qualifica";
}

public class IdmAuthenticationHandler : AuthenticationHandler<IdmAuthenticationOptions>
{
    private const string MyHttpClaimNumeroMatricola = "My-MATRICOLA";
    private const string MyHttpClaimCodiceFiscale = "My-CODICE-FISCALE";
    private const string MyHttpClaimIndirizzoEmail = "My-EMAIL";
    private const string MyHttpClaimWindowsAccount = "My-ACCOUNT-WINDOWS";
    private const string MyHttpClaimNominativo = "My-NOMINATIVO";
    private const string MyHttpClaimNome = "My-NOME";
    private const string MyHttpClaimCognome = "My-COGNOME";
    private const string MyHttpClaimCodiceSede = "My-CODICE-SEDE";
    private const string MyHttpClaimCodiceSedeSap = "My-CODICE-SEDE-SAP";
    private const string MyHttpClaimProfiloVega = "My-PROFILO-VEGA";
    private const string MyHttpClaimQualifica = "My-QUALIFICA";
    private const string MyHttpClaimRuoli = "My-RUOLI";

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

        try
        {
            var windowsAccount = request!.Headers[MyHttpClaimWindowsAccount];
            var numeroMatricola = request!.Headers[MyHttpClaimNumeroMatricola];
            var codiceFiscale = request!.Headers[MyHttpClaimCodiceFiscale];
            var email = request!.Headers[MyHttpClaimIndirizzoEmail];
            var nominativo = request!.Headers[MyHttpClaimNominativo];
            var nome = request!.Headers[MyHttpClaimNome];
            var cognome = request!.Headers[MyHttpClaimCognome];
            var codiceSede = request!.Headers[MyHttpClaimCodiceSede];
            var codiceSedeSap = request!.Headers[MyHttpClaimCodiceSedeSap];
            var vegaProfilo = request!.Headers[MyHttpClaimProfiloVega];
            var qualifica = request!.Headers[MyHttpClaimQualifica];
            var ruoli = request!.Headers[MyHttpClaimRuoli];

            if (string.IsNullOrWhiteSpace(windowsAccount))
            {
                return AuthenticateResult.Fail($"Utente non aMyUserscato");
            }

            if (string.IsNullOrWhiteSpace(numeroMatricola))
            {
                return AuthenticateResult.Fail($"Utente non aMyUserscato");
            }

            var myRoles = new List<string>();
            var userRoles = ruoli.ToString() ?? string.Empty;
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
                new(ClaimTypes.Name, nome.ToString() ?? string.Empty),
                new(ClaimTypes.Surname, cognome.ToString() ?? string.Empty),
                new(ClaimTypes.NameIdentifier, numeroMatricola.ToString() ?? string.Empty),
                new(ClaimTypes.Email, email.ToString() ?? string.Empty),
                new(ClaimTypes.WindowsAccountName, windowsAccount.ToString() ?? string.Empty),
                new(IdmClaimTypes.CodiceFiscale, codiceFiscale.ToString() ?? string.Empty),
                new(IdmClaimTypes.Nominativo, nominativo.ToString() ?? string.Empty),
                new(IdmClaimTypes.CodiceSede, codiceSede.ToString() ?? string.Empty),
                new(IdmClaimTypes.CodiceSedeSap, codiceSedeSap.ToString() ?? string.Empty),
                new(IdmClaimTypes.ProfiloVega, vegaProfilo.ToString() ?? string.Empty),
                new(IdmClaimTypes.Qualifica, qualifica.ToString() ?? string.Empty),
            };

            foreach (var role in myRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity
                (claims, this.Scheme.Name);

            var claimsPrincipal = new ClaimsPrincipal
                (claimsIdentity);

            authorized = true;

            return AuthenticateResult.Success
                (new AuthenticationTicket(claimsPrincipal,
                this.Scheme.Name));
        }
        finally
        {
            base.Logger.LogItem(LogLevel.Information, new
            {
                Headers = request?.Headers,
                Path = request?.Path.Value,
                QueryString = request?.QueryString.Value,
                Authorized = authorized,
                AllowedRoles = allowedRoles
            });
        }
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        await base.HandleChallengeAsync(properties);
        Response.Redirect("/Unauthorized");
    }
}