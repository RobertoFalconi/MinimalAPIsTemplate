namespace MinimalAPIs.Services;

public class MyCompressingService
{
    public async Task<string> Compress(string originalData)
    {
        byte[] compressedData;
        using (var memoryStream = new MemoryStream())
        {
            using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
            {
                await gzipStream.WriteAsync(Encoding.UTF8.GetBytes(originalData), 0, originalData.Length);
            }
            compressedData = memoryStream.ToArray();
        }

        var hexString = BitConverter.ToString(compressedData).Replace("-", "");

        return hexString;
    }

    public async Task<string> Decompress(string compressedData)
    {
        byte[] decompressedData;
        var byteArray = Enumerable.Range(0, compressedData.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(compressedData.Substring(x, 2), 16))
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
