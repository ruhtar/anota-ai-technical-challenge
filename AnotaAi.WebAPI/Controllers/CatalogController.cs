using AnotaAi.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AnotaAi.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService s3Service;

    public CatalogController(ICatalogService s3Service)
    {
        this.s3Service = s3Service;
    }

    [HttpGet("{ownerId}")]
    public async Task<IActionResult> GetCatalog([Required] string ownerId, CancellationToken cancellationToken)
    {
        var catalogJson = await s3Service.GetCatalogJsonAsync(ownerId, cancellationToken);

        if (catalogJson is "")
            return NoContent();

        if (catalogJson is null)
            return Problem();

        return Content(catalogJson, "application/json");
    }
}
