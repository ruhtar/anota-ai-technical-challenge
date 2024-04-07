using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using AnotaAi.Infraestructure.Repositories;

namespace AnotaAi.Application.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> InsertAsync(ProductCreateDto productCreateDto);
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

        public async Task<List<Product>> GetAllAsync()
        {
            return await productRepository.GetAll();
        }

        public async Task<Product> InsertAsync(ProductCreateDto productCreateDto)
        {
            var category = await categoryService.GetById(productCreateDto.CategoryId);
            var product = new Product(productCreateDto, category);
            await productRepository.InsertAsync(product);
            return product;
        }
    }
}
