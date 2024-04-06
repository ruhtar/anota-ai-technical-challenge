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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await productRepository.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto productCreateDto)
        {
            var product = new Product(productCreateDto);

            await productRepository.InsertAsync(product);
            return Ok(product);
        }
    }
}
