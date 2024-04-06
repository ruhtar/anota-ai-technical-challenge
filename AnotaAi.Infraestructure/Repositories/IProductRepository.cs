using AnotaAi.Domain.Entities;

namespace AnotaAi.Infraestructure.Repositories
{
    public interface IProductRepository
    {
        Task InsertPost(Product product);
    }
}