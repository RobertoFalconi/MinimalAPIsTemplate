namespace MinimalAPIs.Services
{
    public interface IMinimalService
    {
        Task<double> Double(double a, CancellationToken token);
    }

    public class MinimalService : IMinimalService
    {
        public async Task<double> Double(double a, CancellationToken token)
        {
            return a * 2;
        }
    }
}
