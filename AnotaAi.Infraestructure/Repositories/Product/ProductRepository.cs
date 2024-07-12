using AnotaAi.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AnotaAi.Infraestructure.Repositories;

public interface IProductRepository
{
    Task DeleteAsync(string id);
    Task<List<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(string id);
    Task InsertAsync(Product product);
    Task UpdateAsync(string id, Product product);
}

public class ProductRepository : IProductRepository
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductRepository()
    {
        _client = new MongoClient("mongodb://localhost:27017"); //TODO: alterar
        _database = _client.GetDatabase("catalog");
        _productsCollection = _database.GetCollection<Product>("products");
    }

    public async Task InsertAsync(Product product)
    {
        await _productsCollection.InsertOneAsync(product);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _productsCollection.Find(Builders<Product>.Filter.Empty).ToListAsync();
    }

    public async Task<Product> GetByIdAsync(string id)
    {
        var filter = Builders<Product>.Filter.Eq("_id", ObjectId.Parse(id));
        return await _productsCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(string id, Product product)
    {
        var filter = Builders<Product>.Filter.Eq("_id", id);

        var updateBuilder = Builders<Product>.Update
            .Set(p => p.Description, product.Description)
            .Set(p => p.Title, product.Title)
            .Set(p => p.Price, product.Price)
            .Set(p => p.CategoryId, product.CategoryId)
            .Set(p => p.OwnerId, product.OwnerId);

        await _productsCollection.UpdateOneAsync(filter, updateBuilder);
    }
    public async Task DeleteAsync(string id)
    {
        var filter = Builders<Product>.Filter.Eq("_id", ObjectId.Parse(id));
        await _productsCollection.DeleteOneAsync(filter);
    }
}
