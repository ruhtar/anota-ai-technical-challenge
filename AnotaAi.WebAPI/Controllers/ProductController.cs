using AnotaAi.Domain.DTOs;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto productCreateDto)
        {
            var category = new Category()
            {
                Title = productCreateDto.Category.Title,
                Description = productCreateDto.Category.Description,
                OwnerId = productCreateDto.Category.OwnerId,
            };
            var product = new Product()
            {
                Category = category,
                Description = productCreateDto.Description,
                OwnerId = productCreateDto.OwnerId,
                Price = productCreateDto.Price,
                Title = productCreateDto.Title
            };
            await productRepository.InsertAsync(product);
            return Ok(product);
        }
    }
}
