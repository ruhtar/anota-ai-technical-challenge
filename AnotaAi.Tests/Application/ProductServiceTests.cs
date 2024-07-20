using AnotaAi.Application.Services;
using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using AnotaAi.Infraestructure.Repositories;
using Moq;
using Xunit;

namespace AnotaAi.Tests.Application;

public class ProductServiceTests
{
    private readonly Mock<ICategoryService> mockCategoryService;
    private readonly Mock<IProductRepository> mockProductRepository;
    private readonly ProductService productService;

    public ProductServiceTests()
    {
        mockCategoryService = new Mock<ICategoryService>();
        mockProductRepository = new Mock<IProductRepository>();
        productService = new ProductService(mockCategoryService.Object, mockProductRepository.Object);
    }

    [Fact]
    public async Task GetAllByOwnerIdAsync_ShouldReturnProducts()
    {
        // Arrange
        var ownerId = "ownerId";
        var products = new List<Product> { new Product { Id = "1" }, new Product { Id = "2" } };
        mockProductRepository.Setup(repo => repo.GetAllByOwnerIdAsync(ownerId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(products);

        // Act
        var result = await productService.GetAllByOwnerIdAsync(ownerId, CancellationToken.None);

        // Assert
        Assert.Equal(products, result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnDeletedProduct()
    {
        // Arrange
        var id = "productId";
        var product = new Product { Id = id };
        mockProductRepository.Setup(repo => repo.DeleteAsync(id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(product);

        // Act
        var result = await productService.DeleteAsync(id, CancellationToken.None);

        // Assert
        Assert.Equal(product, result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product> { new Product { Id = "1" }, new Product { Id = "2" } };
        mockProductRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                             .ReturnsAsync(products);

        // Act
        var result = await productService.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.Equal(products, result);
    }

    [Fact]
    public async Task GetById_ShouldReturnProduct()
    {
        // Arrange
        var id = "productId";
        var product = new Product { Id = id };
        mockProductRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(product);

        // Act
        var result = await productService.GetById(id, CancellationToken.None);

        // Assert
        Assert.Equal(product, result);
    }

    [Fact]
    public async Task InsertAsync_ShouldInsertProduct()
    {
        // Arrange
        var product = new Product { Id = "productId", CategoryId = "categoryId" };
        mockCategoryService.Setup(catService => catService.GetByIdAsync(product.CategoryId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new Category { Id = product.CategoryId });
        mockProductRepository.Setup(repo => repo.InsertAsync(product, It.IsAny<CancellationToken>()))
                             .Returns(Task.CompletedTask);

        // Act
        var result = await productService.InsertAsync(product, CancellationToken.None);

        // Assert
        Assert.Equal(product, result);
    }

    [Fact]
    public async Task InsertAsync_ShouldThrowExceptionIfCategoryNotFound()
    {
        // Arrange
        var product = new Product { Id = "productId", CategoryId = "categoryId" };
        mockCategoryService.Setup(catService => catService.GetByIdAsync(product.CategoryId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Category?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => productService.InsertAsync(product, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct()
    {
        // Arrange
        var id = "productId";
        var updateDto = new UpdateProductDto("", "", 10, "categoryId");
        var updatedProduct = new Product { Id = id };
        mockCategoryService.Setup(catService => catService.GetByIdAsync(updateDto.CategoryId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new Category { Id = updateDto.CategoryId });
        mockProductRepository.Setup(repo => repo.UpdateAsync(id, updateDto, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(updatedProduct);

        // Act
        var result = await productService.UpdateAsync(id, updateDto, CancellationToken.None);

        // Assert
        Assert.Equal(updatedProduct, result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowExceptionIfCategoryNotFound()
    {
        // Arrange
        var id = "productId";
        var updateDto = new UpdateProductDto("", "", 10, "categoryId");
        mockCategoryService.Setup(catService => catService.GetByIdAsync(updateDto.CategoryId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Category?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => productService.UpdateAsync(id, updateDto, CancellationToken.None));
    }
}
