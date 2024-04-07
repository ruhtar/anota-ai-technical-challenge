using AnotaAi.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AnotaAi.Infraestructure.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<Product> GetById(string id);
        Task InsertAsync(Product product);
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

        public async Task<List<Product>> GetAll()
        {
            return await _productsCollection.Find(Builders<Product>.Filter.Empty).ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {
            var filter = Builders<Product>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _productsCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
