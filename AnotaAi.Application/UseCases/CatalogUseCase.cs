using AnotaAi.Application.Services;
using AnotaAi.Domain.Entities;
using System.Text.Json;

namespace AnotaAi.Application.UseCases;

public interface ICatalogUseCase
{
    Task<bool> UpdateOwnerCatalog(string ownerId, CancellationToken cancellationToken);
}

public class CatalogUseCase : ICatalogUseCase
{
    private readonly ICatalogService catalogService;
    private readonly ICategoryService categoryService;
    private readonly IProductService productService;

    public CatalogUseCase(ICatalogService catalogService, IProductService productService, ICategoryService categoryService)
    {
        this.catalogService = catalogService;
        this.productService = productService;
        this.categoryService = categoryService;
    }

    public async Task<bool> UpdateOwnerCatalog(string ownerId, CancellationToken cancellationToken)
    {
        var currentJson = await catalogService.GetCatalogJsonAsync(ownerId, cancellationToken);

        if (currentJson is null)
            return false;

        var categories = await categoryService.GetAllByOwnerIdAsync(ownerId, cancellationToken);

        var products = await productService.GetAllByOwnerIdAsync(ownerId, cancellationToken);

        var catalog = new Catalog
        {
            Owner = ownerId,
            CatalogItems = []
        };

        foreach (var category in categories)
        {
            var catalogCategory = new CatalogCategory
            {
                CategoryTitle = category.Title,
                CategoryDescription = category.Description,
                Items = []
            };

            var categoryProducts = products.Where(p => p.CategoryId == category.Id);

            foreach (var product in categoryProducts)
            {
                var item = new Item
                {
                    Title = product.Title,
                    Description = product.Description,
                    Price = product.Price
                };

                catalogCategory.Items.Add(item);
            }

            catalog.CatalogItems.Add(catalogCategory);
        }

        var newJson = JsonSerializer.Serialize(catalog);

        return await catalogService.SaveToS3StorageAsync(ownerId, newJson, cancellationToken);
    }
}
