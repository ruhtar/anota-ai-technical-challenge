using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AnotaAi.Infraestructure.Repositories;

public interface IProductRepository
{
    Task<Product?> DeleteAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetAllByOwnerIdAsync(string ownerId, CancellationToken cancellationToken);
    Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task InsertAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> UpdateAsync(string id, UpdateProductDto product, CancellationToken cancellationToken);
}

public class ProductRepository : IProductRepository
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductRepository(IConfiguration configuration)
    {
        _client = new MongoClient(configuration.GetConnectionString("Mongo"));
        _database = _client.GetDatabase("catalog");
        _productsCollection = _database.GetCollection<Product>("products");
    }

    public async Task<IEnumerable<Product>> GetAllByOwnerIdAsync(string ownerId, CancellationToken cancellationToken)
    {
        var filter = Builders<Product>.Filter.Eq("ownerId", ownerId);
        return await _productsCollection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task InsertAsync(Product product, CancellationToken cancellationToken) =>
        await _productsCollection.InsertOneAsync(product, null, cancellationToken);

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken) =>
        await _productsCollection.Find(Builders<Product>.Filter.Empty).ToListAsync(cancellationToken);

    public async Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Product>.Filter.Eq("_id", ObjectId.Parse(id));
        return await _productsCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Product?> UpdateAsync(string id, UpdateProductDto product, CancellationToken cancellationToken)
    {
        var filter = Builders<Product>.Filter.Eq("_id", ObjectId.Parse(id));

        var updateBuilder = Builders<Product>.Update;
        var updateDefinitions = new List<UpdateDefinition<Product>>();

        if (!string.IsNullOrEmpty(product.Description))
            updateDefinitions.Add(updateBuilder.Set(p => p.Description, product.Description));

        if (!string.IsNullOrEmpty(product.Title))
            updateDefinitions.Add(updateBuilder.Set(p => p.Title, product.Title));

        if (product.Price != null)
            updateDefinitions.Add(updateBuilder.Set(p => p.Price, product.Price));

        if (!string.IsNullOrEmpty(product.CategoryId))
            updateDefinitions.Add(updateBuilder.Set(p => p.CategoryId, product.CategoryId));

        var combinedUpdate = updateBuilder.Combine(updateDefinitions);

        var options = new FindOneAndUpdateOptions<Product>
        {
            ReturnDocument = ReturnDocument.After
        };

        return await _productsCollection.FindOneAndUpdateAsync(filter, combinedUpdate, options, cancellationToken);
    }

    public async Task<Product?> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Product>.Filter.Eq("_id", ObjectId.Parse(id));
        return await _productsCollection.FindOneAndDeleteAsync(filter, null, cancellationToken);
    }
}
