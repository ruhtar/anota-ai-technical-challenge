using AnotaAi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnotaAi.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService s3Service;

        public CatalogController(ICatalogService s3Service)
        {
            this.s3Service = s3Service;
        }

        //Consider the product catalog as a JSON compilation of all available categories and items owned by a user.This way, the catalog search endpoint does not need to fetch information from the database.

        [HttpGet("catalog")]
        public async Task<IActionResult> GetCatalog(CancellationToken cancellationToken)
        {
            var catalogJson = await s3Service.GetCatalogJsonAsync(cancellationToken);
            return Content(catalogJson, "application/json");
        }
    }
}
