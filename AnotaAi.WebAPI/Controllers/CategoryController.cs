using AnotaAi.Application.Services;
using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AnotaAi.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : Controller
{
    private readonly ICategoryService categoryService;
    private readonly ICatalogProducer catalogService;

    public CategoryController(ICategoryService categoryService, ICatalogProducer catalogService)
    {
        this.categoryService = categoryService;
        this.catalogService = catalogService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await categoryService.GetByIdAsync(id, cancellationToken);

        if (result is null)
            return NoContent();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await categoryService.GetAllAsync(cancellationToken);

        if (result is null)
            return NoContent();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryDto categoryCreateDto, CancellationToken cancellationToken)
    {
        var category = new Category(categoryCreateDto);
        catalogService.PublishEvent(categoryCreateDto.OwnerId);
        return Ok(await categoryService.InsertAsync(category, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        await categoryService.DeleteAsync(id, cancellationToken);
        catalogService.PublishEvent(id);
        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> Update([Required] string id, [FromBody] CategoryDto categoryDto, CancellationToken cancellationToken)
    {
        var category = new Category(categoryDto);
        await categoryService.UpdateAsync(id, category, cancellationToken);
        catalogService.PublishEvent(categoryDto.OwnerId);
        return Ok();
    }
}
