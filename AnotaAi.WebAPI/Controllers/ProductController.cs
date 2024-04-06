using AnotaAi.Domain.Entities;
using AnotaAi.Infraestructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AnotaAi.WebAPI.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Create()
        {
            Product produto = new Product
            {
                Title = "Smartphone",
                Description = "Brand new smartphone with latest features.",
                Price = 999.99m,
                Category = "Electronics",
                OwnerId = "user123"
            };
            await productRepository.InsertPost(produto);
            return Ok(produto);
        }
    }
}
