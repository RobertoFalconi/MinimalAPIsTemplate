namespace MinimalAPIs.Handlers.ServiceHandlers;

public sealed record CompressRequest(string OriginalData) : IRequest<string>;
public sealed record DecompressRequest(string CompressedData) : IRequest<string>;

public sealed class CompressingHandler : IRequestHandler<CompressRequest, string>,
                                         IRequestHandler<DecompressRequest, string>
{

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

    public async Task<string> Handle(DecompressRequest request, CancellationToken cancellationToken)
    {
        byte[] decompressedData;
        var byteArray = Enumerable.Range(0, request.CompressedData.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(request.CompressedData.Substring(x, 2), 16))
                             .ToArray();
        using (var memoryStream = new MemoryStream(byteArray))
        {
            using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            using var decompressedStream = new MemoryStream();
            await gzipStream.CopyToAsync(decompressedStream);
            decompressedData = decompressedStream.ToArray();
        }
        return Encoding.UTF8.GetString(decompressedData);
    }
}