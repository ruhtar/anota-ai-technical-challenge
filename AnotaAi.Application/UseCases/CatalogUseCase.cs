using AnotaAi.Application.Services;

namespace AnotaAi.Application.UseCases;

public interface ICatalogUseCase
{
    Task<bool> UpdateOwnerCatalog(string ownerId, CancellationToken cancellationToken);
}

public class CatalogUseCase : ICatalogUseCase
{
    private readonly ICatalogService catalogService;

    public CatalogUseCase(ICatalogService catalogService)
    {
        this.catalogService = catalogService;
    }

    public async Task<bool> UpdateOwnerCatalog(string ownerId, CancellationToken cancellationToken)
    {
        //Buscar o Catalog Json no S3 pelo nome do owner. Caso não exista, apenas criar o catalog com as informações da base??
        //
        var json = await catalogService.GetCatalogJsonAsync(ownerId, cancellationToken);

        if (json is "")
            return await catalogService.UpdateCatalogJsonAsync(ownerId, json, cancellationToken);

        return false;
    }
}
