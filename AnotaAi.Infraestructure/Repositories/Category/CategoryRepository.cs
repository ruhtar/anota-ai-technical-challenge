using AnotaAi.Domain.Entities;
using MongoDB.Driver;

namespace AnotaAi.Infraestructure.Repositories
{
    public interface ICategoryRepository
    {
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

        public async Task InsertAsync(Category category)
        {
            await _categoryCollection.InsertOneAsync(category);
        }
    }
}
