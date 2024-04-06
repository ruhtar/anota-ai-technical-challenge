using AnotaAi.Domain.Entities;
using MongoDB.Driver;

namespace AnotaAi.Infraestructure.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task InsertAsync(Product product);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Product> _productsCollection;
        private readonly ICategoryRepository _categoryRepository;

        public ProductRepository(ICategoryRepository categoryRepository)
        {
            _client = new MongoClient("mongodb://localhost:27017"); //TODO: alterar
            _database = _client.GetDatabase("catalog");
            _productsCollection = _database.GetCollection<Product>("products");
            _categoryRepository = categoryRepository;
        }

        public async Task InsertAsync(Product product)
        {
            await _productsCollection.InsertOneAsync(product);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _productsCollection.Find(Builders<Product>.Filter.Empty).ToListAsync();
        }

        public async Task Associate(Product product, Category category)
        {

        }
    }
}
