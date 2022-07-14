namespace MinimalAPIs.Services
{
    public interface IMyTokenService
    {
        Task<double> Double(double a, CancellationToken token);
    }

    public class MyTokenService : IMyTokenService
    {
        public async Task<double> Double(double a, CancellationToken token)
        {
            return a * 2;
        }
    }
}
