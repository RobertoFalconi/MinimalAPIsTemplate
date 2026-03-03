namespace MinimalAPIs.Filters;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Add your Hangfire's dashboard authorization logic here.
        return true;
    }
}