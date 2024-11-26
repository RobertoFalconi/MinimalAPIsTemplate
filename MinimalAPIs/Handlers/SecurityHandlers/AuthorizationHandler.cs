namespace MinimalAPIs.Handlers.SecurityHandlers;

public sealed class AuthorizationRequirement : IAuthorizationRequirement { }

public sealed class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
{
    protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
    {
        using (var dbContext = new MinimalApisDbContext())
        {
            using var dbContextTransaction = await dbContext.Database.BeginTransactionAsync();
            // Add your authorization logic here.
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}