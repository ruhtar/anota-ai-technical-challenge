﻿using AnotaAi.Domain.Entities;
using AnotaAi.Infraestructure.Repositories;

namespace AnotaAi.Application.Services;

public interface IProductService
{
    Task DeleteAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetAllByOwnerIdAsync(string ownerId, CancellationToken cancellationToken);
    Task<Product> GetById(string id, CancellationToken cancellationToken);
    Task<Product> InsertAsync(Product productCreateDto, CancellationToken cancellationToken);
    Task UpdateAsync(string id, Product product, CancellationToken cancellationToken);
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

    public async Task<IEnumerable<Product>> GetAllByOwnerIdAsync(string ownerId, CancellationToken cancellationToken) =>
        await productRepository.GetAllByOwnerIdAsync(ownerId, cancellationToken);

    public async Task DeleteAsync(string id, CancellationToken cancellationToken) => await productRepository.DeleteAsync(id, cancellationToken);

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken) => await productRepository.GetAllAsync(cancellationToken);

    public async Task<Product> GetById(string id, CancellationToken cancellationToken) => await productRepository.GetByIdAsync(id, cancellationToken);

    public async Task<Product> InsertAsync(Product product, CancellationToken cancellationToken)
    {
        var _ = await categoryService.GetByIdAsync(product.CategoryId, cancellationToken) ?? throw new Exception("Category not found");
        await productRepository.InsertAsync(product, cancellationToken);
        return product;
    }
    public async Task UpdateAsync(string id, Product product, CancellationToken cancellationToken)
    {
        var _ = await categoryService.GetByIdAsync(product.CategoryId, cancellationToken) ?? throw new Exception("Category not found");
        await productRepository.UpdateAsync(id, product, cancellationToken);
    }
}
