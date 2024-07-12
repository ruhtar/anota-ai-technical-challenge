using AnotaAi.Application.Services;
using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AnotaAi.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //todo: todo endpoint de alteração(post,put,delete) de produto/categoria deve publicar um evento de alteração para alterar o catalogo no S3
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await categoryService.GetById(id);

            if (result is null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await categoryService.GetAllAsync();

            if (result is null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryCreateDto)
        {
            var category = new Category(categoryCreateDto);

            return Ok(await categoryService.InsertAsync(category));
        }
    }
}
