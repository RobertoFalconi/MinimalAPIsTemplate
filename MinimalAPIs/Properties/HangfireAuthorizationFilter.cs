namespace MinimalAPIs.Properties;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        //return context.GetHttpContext().User.Identity.IsAuthenticated;
        return true;
    }
}