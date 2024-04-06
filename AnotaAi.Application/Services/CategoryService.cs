using AnotaAi.Domain.Entities;
using AnotaAi.Infraestructure.Repositories;

namespace AnotaAi.Application.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetById(string id);
        Task InsertAsync();
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await categoryRepository.GetAll();
        }

        public async Task<Category> GetById(string id)
        {
            return await categoryRepository.GetById(id);
        }

        public async Task InsertAsync()
        {

        }


    }
}
