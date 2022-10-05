namespace MinimalAPIs.Services
{
    public class MyTokenService
    {
        public async Task<double> Double(double a, CancellationToken token)
        {
            return a * 2;
        }
    }
}
