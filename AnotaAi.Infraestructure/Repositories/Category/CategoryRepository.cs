using AnotaAi.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AnotaAi.Infraestructure.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category> GetByIdAsync(string id);
    Task InsertAsync(Category category);
    Task UpdateAsync(string id, Category category);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Category> _categoryCollection;

    public CategoryRepository()
    {
        _client = new MongoClient("mongodb://localhost:27017"); //TODO: alterar
        _database = _client.GetDatabase("catalog");
        _categoryCollection = _database.GetCollection<Category>("categories");
    }

    public async Task UpdateAsync(string id, Category category)
    {
        var filter = Builders<Category>.Filter.Eq(category => category.Id, id);
        var update = Builders<Category>.Update.Set(category => category, category);

        await _categoryCollection.UpdateOneAsync(filter, update);
    }

    public async Task<Category> GetByIdAsync(string id)
    {
        var filter = Builders<Category>.Filter.Eq("_id", ObjectId.Parse(id));
        return await _categoryCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _categoryCollection.Find(Builders<Category>.Filter.Empty).ToListAsync();
    }

    public async Task InsertAsync(Category category)
    {
        await _categoryCollection.InsertOneAsync(category);
    }
}
