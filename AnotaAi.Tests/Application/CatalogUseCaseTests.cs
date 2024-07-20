using AnotaAi.Application.Services;
using AnotaAi.Application.UseCases;
using AnotaAi.Domain.Entities;
using Moq;
using System.Text.Json;
using Xunit;

namespace AnotaAi.Tests.Application;

public class CatalogUseCaseTests
{
    [Fact]
    public async Task UpdateOwnerCatalog_ReturnsFalse_WhenCurrentJsonIsNull()
    {
        // Arrange
        var ownerId = "owner123";
        var cancellationToken = CancellationToken.None;

        var catalogServiceMock = new Mock<ICatalogService>();
        catalogServiceMock.Setup(cs => cs.GetCatalogJsonAsync(ownerId, cancellationToken))
                          .ReturnsAsync((string)null);

        var categoryServiceMock = new Mock<ICategoryService>();
        var productServiceMock = new Mock<IProductService>();

        var sut = new CatalogUseCase(catalogServiceMock.Object, productServiceMock.Object, categoryServiceMock.Object);

        // Act
        var result = await sut.UpdateOwnerCatalog(ownerId, cancellationToken);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateOwnerCatalog_ReturnsTrue_WhenCatalogIsSuccessfullyUpdated()
    {
        // Arrange
        var ownerId = "owner123";
        var cancellationToken = CancellationToken.None;

        var currentJson = "{}";
        var categories = new List<Category>
        {
            new() { Id = "cat1", Title = "Category1", Description = "Description1" }
        };
        var products = new List<Product>
        {
            new Product { Id = "prod1", Title = "Product1", Description = "Description1", Price = 10, CategoryId = "cat1" }
        };

        var catalogServiceMock = new Mock<ICatalogService>();
        catalogServiceMock.Setup(cs => cs.GetCatalogJsonAsync(ownerId, cancellationToken))
                          .ReturnsAsync(currentJson);
        catalogServiceMock.Setup(cs => cs.SaveToS3StorageAsync(ownerId, It.IsAny<string>(), cancellationToken))
                          .ReturnsAsync(true);

        var categoryServiceMock = new Mock<ICategoryService>();
        categoryServiceMock.Setup(cs => cs.GetAllByOwnerIdAsync(ownerId, cancellationToken))
                           .ReturnsAsync(categories);

        var productServiceMock = new Mock<IProductService>();
        productServiceMock.Setup(ps => ps.GetAllByOwnerIdAsync(ownerId, cancellationToken))
                          .ReturnsAsync(products);

        var sut = new CatalogUseCase(catalogServiceMock.Object, productServiceMock.Object, categoryServiceMock.Object);

        // Act
        var result = await sut.UpdateOwnerCatalog(ownerId, cancellationToken);

        // Assert
        Assert.True(result);

        catalogServiceMock.Verify(cs => cs.SaveToS3StorageAsync(ownerId, It.IsAny<string>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateOwnerCatalog_CreatesExpectedCatalogJson()
    {
        // Arrange
        var ownerId = "owner123";
        var cancellationToken = CancellationToken.None;

        var currentJson = "{}";
        var categories = new List<Category>
        {
            new() { Id = "cat1", Title = "Category1", Description = "Description1" }
        };
        var products = new List<Product>
        {
            new() { Id = "prod1", Title = "Product1", Description = "Description1", Price = 10, CategoryId = "cat1" }
        };

        var catalogServiceMock = new Mock<ICatalogService>();
        catalogServiceMock.Setup(cs => cs.GetCatalogJsonAsync(ownerId, cancellationToken))
                          .ReturnsAsync(currentJson);

        var categoryServiceMock = new Mock<ICategoryService>();
        categoryServiceMock.Setup(cs => cs.GetAllByOwnerIdAsync(ownerId, cancellationToken))
                           .ReturnsAsync(categories);

        var productServiceMock = new Mock<IProductService>();
        productServiceMock.Setup(ps => ps.GetAllByOwnerIdAsync(ownerId, cancellationToken))
                          .ReturnsAsync(products);

        var sut = new CatalogUseCase(catalogServiceMock.Object, productServiceMock.Object, categoryServiceMock.Object);

        // Act
        var result = await sut.UpdateOwnerCatalog(ownerId, cancellationToken);

        // Assert
        var expectedCatalog = new Catalog
        {
            Owner = ownerId,
            CatalogItems =
            [
                new()
                {
                    CategoryTitle = "Category1",
                    CategoryDescription = "Description1",
                    Items =
                    [
                        new()
                        {
                            Title = "Product1",
                            Description = "Description1",
                            Price = 10
                        }
                    ]
                }
            ]
        };
        var expectedJson = JsonSerializer.Serialize(expectedCatalog);

        catalogServiceMock.Verify(cs => cs.SaveToS3StorageAsync(ownerId, expectedJson, cancellationToken), Times.Once);
    }
}

