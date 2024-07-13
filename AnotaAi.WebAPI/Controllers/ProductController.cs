using AnotaAi.Application.Services;
using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AnotaAi.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : Controller
{
    private readonly IProductService productService;
    private readonly ICatalogProducer catalogService;

    public ProductController(IProductService productService, ICatalogProducer catalogService)
    {
        this.productService = productService;
        this.catalogService = catalogService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await productService.GetById(id, cancellationToken);

        if (result is null)
            return NoContent();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await productService.GetAllAsync(cancellationToken);

        if (result is null)
            return NoContent();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto, CancellationToken cancellationToken)
    {
        var product = new Product(createProductDto);
        var result = await productService.InsertAsync(product, cancellationToken);
        catalogService.PublishEvent(createProductDto.OwnerId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch]
    public async Task<IActionResult> Update([Required] string id, [FromBody] UpdateProductDto productDto, CancellationToken cancellationToken)
    {
        var product = await productService.UpdateAsync(id, productDto, cancellationToken);

        if (product is null)
            return Problem();

        catalogService.PublishEvent(product.OwnerId);
        return Ok(product);
    }

    //TODO: WHAT SHOULD HAPPEN TO THE PRODUCTS OF A CATEGORY WHEN THE CATEGORY IS DELETED?
    [HttpDelete]
    public async Task<IActionResult> Delete([Required] string id, CancellationToken cancellationToken)
    {
        var product = await productService.DeleteAsync(id, cancellationToken);

        if (product is null)
            return Problem();

        catalogService.PublishEvent(product.OwnerId);
        return Ok(product);
    }
}
