using AnotaAi.Application.Services;
using AnotaAi.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AnotaAi.WebAPI.Controllers;

[Route("[controller]")]
public class ProductController : Controller
{
    private readonly IProductService productService;

    public ProductController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await productService.GetById(id);

        if (result is null)
            return NoContent();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await productService.GetAllAsync();

        if (result is null)
            return NoContent();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto productCreateDto)
    {
        var result = await productService.InsertAsync(productCreateDto);
        return Ok(result);
    }
}
