using AnotaAi.Domain.Entities;
using MongoDB.Driver;

namespace AnotaAi.Infraestructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Product> _productsCollection;

        public ProductRepository(string connectionString)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("catalog");
            _productsCollection = _database.GetCollection<Product>("products");
        }

        public async Task InsertPost(Product product)
        {
            await _productsCollection.InsertOneAsync(product);
        }
    }
}
