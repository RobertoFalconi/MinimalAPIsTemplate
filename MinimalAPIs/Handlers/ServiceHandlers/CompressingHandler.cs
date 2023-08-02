namespace MinimalAPIs.Handlers.ServiceHandlers;

public record CompressRequest(string OriginalData) : IRequest<string>;

public class CompressingHandler :
    IRequestHandler<CompressRequest, string>
{
    private readonly string _connectionString;

    public CompressingHandler(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MinimalAPIsDB")!;
    }

    public async Task<string> Handle(CompressRequest request, CancellationToken cancellationToken)
    {
        byte[] compressedData;
        using (var memoryStream = new MemoryStream())
        {
            using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
            {
                await gzipStream.WriteAsync(Encoding.UTF8.GetBytes(request.OriginalData), 0, request.OriginalData.Length);
            }
            compressedData = memoryStream.ToArray();
        }

        var hexString = BitConverter.ToString(compressedData).Replace("-", "");

        return hexString;
    }
}