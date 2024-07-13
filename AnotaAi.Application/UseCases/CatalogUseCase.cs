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
        //Buscar o Catalog Json no S3 pelo nome do owner. Caso não exista, apenas criar o catalog com as informações da base??
        var json = await catalogService.GetCatalogJsonAsync(ownerId, cancellationToken);

        //if (json is "")
        //    return await catalogService.SaveToS3StorageAsync(ownerId, json, cancellationToken);

        if (json is null)
            return false;

        //Como já existe, precisamos buscar toda a lista de produtos e categorias de um determinado OwnerId.
        var categories = await categoryService.GetAllByOwnerIdAsync(ownerId, cancellationToken);

        var products = await productService.GetAllByOwnerIdAsync(ownerId, cancellationToken);
        //Montar um Json no formato

        // Montar a estrutura de dados no formato necessário
        var catalog = new Catalog
        {
            Owner = ownerId,  // Supondo que ownerId seja uma string
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

            // Filtrar produtos por categoria
            var categoryProducts = products.Where(p => p.CategoryId == category.Id).ToList();

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

        // Converter para JSON
        var newJson = JsonSerializer.Serialize(catalog);

        //Publicar no S3
        return await catalogService.SaveToS3StorageAsync(ownerId, newJson, cancellationToken);
    }
}
