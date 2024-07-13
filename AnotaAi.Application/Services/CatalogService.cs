using Amazon;
using Amazon.Runtime;
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
    private readonly string _bucketName;
    private readonly string _jsonFileName;

    public CatalogService(IConfiguration configuration)
    {
        _bucketName = configuration["AWS:BucketName"] ?? throw new Exception();
        _jsonFileName = configuration["AWS:JsonFileName"] ?? throw new Exception();
    }

    public async Task<string> GetCatalogJsonAsync(CancellationToken cancellationToken)
    {
        var credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"), Environment.GetEnvironmentVariable("AWS_SECRET_KEY"));

        var s3Client = new AmazonS3Client(credentials, RegionEndpoint.SAEast1);

        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = _jsonFileName
        };

        using var response = await s3Client.GetObjectAsync(request, cancellationToken);

        using var reader = new StreamReader(response.ResponseStream);

        return await reader.ReadToEndAsync(cancellationToken);
    }
}
