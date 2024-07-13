using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AnotaAi.Infraestructure.Repositories;

public interface ICategoryRepository
{
    Task<Category?> DeleteAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken);
    Task<Category> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetAllByOwnerIdAsync(string ownerId, CancellationToken cancellationToken);
    Task InsertAsync(Category category, CancellationToken cancellationToken);
    Task<Category?> UpdateAsync(string id, UpdateCategoryDto category, CancellationToken cancellationToken);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Category> _categoryCollection;

    public CategoryRepository(IConfiguration configuration)
    {
        _client = new MongoClient(configuration.GetConnectionString("Mongo"));
        _database = _client.GetDatabase("catalog");
        _categoryCollection = _database.GetCollection<Category>("categories");
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken)
        => await _categoryCollection.Find(Builders<Category>.Filter.Empty).ToListAsync(cancellationToken);

    public async Task InsertAsync(Category category, CancellationToken cancellationToken)
        => await _categoryCollection.InsertOneAsync(category, null, cancellationToken);

    public async Task<Category?> UpdateAsync(string id, UpdateCategoryDto category, CancellationToken cancellationToken)
    {
        var filter = Builders<Category>.Filter.Eq(c => c.Id, id);
        var updateBuilder = Builders<Category>.Update
            .Set(c => c.Title, category.Title)
            .Set(c => c.Description, category.Description);

        var options = new FindOneAndUpdateOptions<Category>
        {
            ReturnDocument = ReturnDocument.After
        };

        return await _categoryCollection.FindOneAndUpdateAsync(filter, updateBuilder, options, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllByOwnerIdAsync(string ownerId, CancellationToken cancellationToken)
    {
        var filter = Builders<Category>.Filter.Eq("ownerId", ownerId);
        return await _categoryCollection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<Category> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Category>.Filter.Eq("_id", ObjectId.Parse(id));
        return await _categoryCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Category?> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Category>.Filter.Eq("_id", ObjectId.Parse(id));

        return await _categoryCollection.FindOneAndDeleteAsync(filter, null, cancellationToken);
    }
}
