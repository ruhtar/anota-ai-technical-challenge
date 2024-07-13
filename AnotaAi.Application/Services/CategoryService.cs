using AnotaAi.Domain.Entities;
using AnotaAi.Infraestructure.Repositories;

namespace AnotaAi.Application.Services;

public interface ICategoryService
{
    Task DeleteAsync(string id, CancellationToken cancellationToken);
    Task<List<Category>> GetAllAsync(CancellationToken cancellationToken);
    Task<Category> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Category> InsertAsync(Category categoryCreateDto, CancellationToken cancellationToken);
    Task UpdateAsync(string id, Category category, CancellationToken cancellationToken);
}

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }
    public async Task<Category> InsertAsync(Category category, CancellationToken cancellationToken)
    {
        await categoryRepository.InsertAsync(category, cancellationToken);
        return category;
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken)
        => await categoryRepository.GetAllAsync(cancellationToken);

    public async Task UpdateAsync(string id, Category category, CancellationToken cancellationToken)
        => await categoryRepository.UpdateAsync(id, category, cancellationToken);

    public async Task<Category> GetByIdAsync(string id, CancellationToken cancellationToken)
        => await categoryRepository.GetByIdAsync(id, cancellationToken);

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        => await categoryRepository.DeleteAsync(id, cancellationToken);
}
