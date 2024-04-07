using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using AnotaAi.Infraestructure.Repositories;

namespace AnotaAi.Application.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetById(string id);
        Task<Category> InsertAsync(CategoryCreateDto categoryCreateDto);
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

        public async Task<Category> InsertAsync(CategoryCreateDto categoryCreateDto)
        {
            var category = new Category(categoryCreateDto);
            await categoryRepository.InsertAsync(category);
            return category;
        }
    }
}
