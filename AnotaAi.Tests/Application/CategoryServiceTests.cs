using AnotaAi.Application.Services;
using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using AnotaAi.Infraestructure.Repositories;
using Moq;
using Xunit;

namespace AnotaAi.Tests.Application;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> mockCategoryRepository;
    private readonly CategoryService categoryService;

    public CategoryServiceTests()
    {
        mockCategoryRepository = new Mock<ICategoryRepository>();
        categoryService = new CategoryService(mockCategoryRepository.Object);
    }

    [Fact]
    public async Task InsertAsync_ShouldInsertCategoryAndReturnIt()
    {
        // Arrange
        var category = new Category { Id = "categoryId" };
        mockCategoryRepository.Setup(repo => repo.InsertAsync(category, It.IsAny<CancellationToken>()))
                             .Returns(Task.CompletedTask);

        // Act
        var result = await categoryService.InsertAsync(category, CancellationToken.None);

        // Assert
        Assert.Equal(category, result);
    }

    [Fact]
    public async Task GetAllByOwnerIdAsync_ShouldReturnCategories()
    {
        // Arrange
        var ownerId = "ownerId";
        var categories = new List<Category> { new Category { Id = "1" }, new Category { Id = "2" } };
        mockCategoryRepository.Setup(repo => repo.GetAllByOwnerIdAsync(ownerId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(categories);

        // Act
        var result = await categoryService.GetAllByOwnerIdAsync(ownerId, CancellationToken.None);

        // Assert
        Assert.Equal(categories, result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        // Arrange
        var categories = new List<Category> { new Category { Id = "1" }, new Category { Id = "2" } };
        mockCategoryRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                             .ReturnsAsync(categories);

        // Act
        var result = await categoryService.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.Equal(categories, result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategoryAndReturnIt()
    {
        // Arrange
        var id = "categoryId";
        var updateDto = new UpdateCategoryDto(Title: "title", Description: "description");
        var updatedCategory = new Category { Id = id };
        mockCategoryRepository.Setup(repo => repo.UpdateAsync(id, updateDto, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(updatedCategory);

        // Act
        var result = await categoryService.UpdateAsync(id, updateDto, CancellationToken.None);

        // Assert
        Assert.Equal(updatedCategory, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategory()
    {
        // Arrange
        var id = "categoryId";
        var category = new Category { Id = id };
        mockCategoryRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(category);

        // Act
        var result = await categoryService.GetByIdAsync(id, CancellationToken.None);

        // Assert
        Assert.Equal(category, result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnDeletedCategory()
    {
        // Arrange
        var id = "categoryId";
        var category = new Category { Id = id };
        mockCategoryRepository.Setup(repo => repo.DeleteAsync(id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(category);

        // Act
        var result = await categoryService.DeleteAsync(id, CancellationToken.None);

        // Assert
        Assert.Equal(category, result);
    }
}
