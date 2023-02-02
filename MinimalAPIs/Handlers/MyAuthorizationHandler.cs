namespace MinimalAPIs.Handlers;

public class MyAuthorizationRequirement : IAuthorizationRequirement { }

public class MyAuthorizationHandler : AuthorizationHandler<MyAuthorizationRequirement>
{
    protected override async Task<Task> HandleRequirementAsync(
        AuthorizationHandlerContext context, MyAuthorizationRequirement requirement)
    {
        using (var dbContext = new MinimalDbContext())
        {
            using var dbContextTransaction = await dbContext.Database.BeginTransactionAsync();
            // Add your authorization logic here.
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}