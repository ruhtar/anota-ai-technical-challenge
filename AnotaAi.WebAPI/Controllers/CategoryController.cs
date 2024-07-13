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
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto categoryCreateDto, CancellationToken cancellationToken)
    {
        var category = new Category(categoryCreateDto);
        var result = await categoryService.InsertAsync(category, cancellationToken);
        catalogService.PublishEvent(categoryCreateDto.OwnerId);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        await categoryService.DeleteAsync(id, cancellationToken);
        catalogService.PublishEvent(id);
        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> Update([Required] string id, [FromBody] UpdateCategoryDto categoryDto, CancellationToken cancellationToken)
    {
        var category = await categoryService.UpdateAsync(id, categoryDto, cancellationToken);

        if (category is null)
            return Problem();

        catalogService.PublishEvent(category.OwnerId);
        return Ok();
    }
}
