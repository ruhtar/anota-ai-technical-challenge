using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace AnotaAi.Application.Services;

public interface ICatalogService
{
    Task<string?> GetCatalogJsonAsync(string ownerId, CancellationToken cancellationToken);
    Task<bool> SaveToS3StorageAsync(string ownerId, string jsonContent, CancellationToken cancellationToken);
}

//TODO: CREATE A REPOSITORY TO THIS
public class CatalogService : ICatalogService
{
    private readonly string _bucketName;

    public CatalogService()
    {
        _bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME") ?? throw new Exception();
    }

    public async Task<string?> GetCatalogJsonAsync(string ownerId, CancellationToken cancellationToken)
    {
        try
        {
            var credentials = new BasicAWSCredentials(null, null);

            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.SAEast1);

            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = ownerId + ".json"
            };

            using var response = await s3Client.GetObjectAsync(request, cancellationToken);

            using var reader = new StreamReader(response.ResponseStream);

            return await reader.ReadToEndAsync(cancellationToken);
        }
        catch (AmazonS3Exception)
        {
            return string.Empty;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> SaveToS3StorageAsync(string ownerId, string jsonContent, CancellationToken cancellationToken)
    {
        try
        {
            var credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"), Environment.GetEnvironmentVariable("AWS_SECRET_KEY"));

            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.SAEast1);

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = ownerId + ".json",
                ContentBody = jsonContent,
                ContentType = "application/json"
            };

            var response = await s3Client.PutObjectAsync(request, cancellationToken);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        //catch (AmazonS3Exception)
        //{
        //    return false;
        //}
        catch (Exception)
        {
            return false;
        }
    }
}
