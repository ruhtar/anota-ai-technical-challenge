using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

namespace AnotaAi.Application.Services;

public interface ICatalogService
{
    Task<string> GetCatalogJsonAsync(CancellationToken cancellationToken);
}

public class CatalogService : ICatalogService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _jsonFileName;

    public CatalogService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["AWS:BucketName"] ?? throw new Exception();
        _jsonFileName = configuration["AWS:JsonFileName"] ?? throw new Exception();
    }

    public async Task<string> GetCatalogJsonAsync(CancellationToken cancellationToken)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = _jsonFileName
        };

        using var response = await _s3Client.GetObjectAsync(request, cancellationToken);
        using var reader = new StreamReader(response.ResponseStream);
        return await reader.ReadToEndAsync(cancellationToken);
    }
}
