using AnotaAi.Domain.Entities;
using AnotaAi.Infraestructure.Repositories;

namespace AnotaAi.Application.Services;

public interface IProductService
{
    Task DeleteAsync(string id);
    Task<List<Product>> GetAllAsync();
    Task<Product> GetById(string id);
    Task<Product> InsertAsync(Product productCreateDto);
    Task UpdateAsync(string id, Product product);
}

public class ProductService : IProductService
{
    private readonly ICategoryService categoryService;
    private readonly IProductRepository productRepository;

    public ProductService(ICategoryService categoryService, IProductRepository productRepository)
    {
        this.categoryService = categoryService;
        this.productRepository = productRepository;
    }

    public async Task DeleteAsync(string id) => await productRepository.DeleteAsync(id);

    public async Task<List<Product>> GetAllAsync() => await productRepository.GetAllAsync();

    public async Task<Product> GetById(string id) => await productRepository.GetByIdAsync(id);

    public async Task<Product> InsertAsync(Product product)
    {
        var _ = await categoryService.GetByIdAsync(product.CategoryId) ?? throw new Exception("Category not found");
        await productRepository.InsertAsync(product);
        return product;
    }
    public async Task UpdateAsync(string id, Product product)
    {
        var _ = await categoryService.GetByIdAsync(product.CategoryId) ?? throw new Exception("Category not found");
        await productRepository.UpdateAsync(id, product);
    }
}
