using AnotaAi.Domain.Entities;
using AnotaAi.Infraestructure.Repositories;

namespace AnotaAi.Application.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<Category> GetById(string id);
    Task<Category> InsertAsync(Category categoryCreateDto);
    Task UpdateAsync(string id, Category category);
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
        return await categoryRepository.GetAllAsync();
    }

    public async Task UpdateAsync(string id, Category category)
    {
        await categoryRepository.UpdateAsync(id, category);
    }

    public async Task<Category> GetById(string id)
    {
        return await categoryRepository.GetByIdAsync(id);
    }

    public async Task<Category> InsertAsync(Category category)
    {
        await categoryRepository.InsertAsync(category);
        return category;
    }
}
