using Microsoft.AspNetCore.Mvc;

namespace AnotaAi.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        //Consider the product catalog as a JSON compilation of all available categories and items owned by a user.This way, the catalog search endpoint does not need to fetch information from the database.
    }
}
