using AnotaAi.Application.Services;
using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AnotaAi.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //todo: todo endpoint de alteração(post,put,delete) de produto/categoria deve publicar um evento de alteração para alterar o catalogo no S3
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly ICatalogService catalogService;

        public CategoryController(ICategoryService categoryService, ICatalogService catalogService)
        {
            this.categoryService = categoryService;
            this.catalogService = catalogService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await categoryService.GetByIdAsync(id);

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
            catalogService.PublishEvent([categoryCreateDto.OwnerId]);
            return Ok(await categoryService.InsertAsync(category));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await categoryService.DeleteAsync(id);
            catalogService.PublishEvent([id]);
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Update([Required] string id, [FromBody] CategoryDto categoryDto)
        {
            var category = new Category(categoryDto);
            await categoryService.UpdateAsync(id, category);
            catalogService.PublishEvent([categoryDto.OwnerId]);
            return Ok();
        }
    }
}
