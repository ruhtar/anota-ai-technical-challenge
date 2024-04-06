﻿using AnotaAi.Domain.Entities;
using MongoDB.Driver;

namespace AnotaAi.Infraestructure.Repositories
{
    public interface IProductRepository
    {
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
    }
}