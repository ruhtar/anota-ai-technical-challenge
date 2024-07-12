using AnotaAi.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AnotaAi.Infraestructure.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAll();
    Task<Category> GetById(string id);
    Task InsertAsync(Category category);
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

    public async Task<Category> GetById(string id)
    {
        var filter = Builders<Category>.Filter.Eq("_id", ObjectId.Parse(id));
        return await _categoryCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<Category>> GetAll()
    {
        return await _categoryCollection.Find(Builders<Category>.Filter.Empty).ToListAsync();
    }

    public async Task InsertAsync(Category category)
    {
        await _categoryCollection.InsertOneAsync(category);
    }
}
