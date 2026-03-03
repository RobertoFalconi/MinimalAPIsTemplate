namespace MinimalSPAwithAPIs.Handlers.SecurityHandlers;

public class CurrentProfileHandler : AuthorizationHandler<CurrentProfileRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CurrentProfileRequirement requirement)
    {
        var profileClaim = context.User.FindFirst(IdmClaimTypes.ProfiloCorrente);
        if (profileClaim != null && profileClaim.Value == requirement.RequiredProfile)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

public class CurrentProfileRequirement : IAuthorizationRequirement
{
    public string RequiredProfile { get; }

    public CurrentProfileRequirement(string requiredProfile)
    {
        RequiredProfile = requiredProfile;
    }
}
