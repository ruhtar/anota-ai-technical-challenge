using AnotaAi.Application.Services;
using AnotaAi.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AnotaAi.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            return Ok(await categoryService.GetById(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await categoryService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto categoryCreateDto)
        {
            return Ok(await categoryService.InsertAsync(categoryCreateDto));
        }
    }
}
