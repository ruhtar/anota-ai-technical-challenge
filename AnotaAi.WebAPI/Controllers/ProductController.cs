using AnotaAi.Application.Services;
using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AnotaAi.WebAPI.Controllers;

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
    public async Task<IActionResult> Create([FromBody] ProductDto productCreateDto, CancellationToken cancellationToken)
    {
        var product = new Product(productCreateDto);
        var result = await productService.InsertAsync(product, cancellationToken);
        catalogService.PublishEvent(productCreateDto.OwnerId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    //todo: make sure that only the required fileds can be passed to the endpoint (PATCH)
    [HttpPatch]
    public async Task<IActionResult> Update([Required] string id, [FromBody] ProductDto productCreateDto, CancellationToken cancellationToken)
    {
        var product = new Product(productCreateDto);
        await productService.UpdateAsync(id, product, cancellationToken);
        catalogService.PublishEvent(productCreateDto.OwnerId);
        return Ok();
    }

    //TODO: WHAT SHOULD HAPPEN TO THE PRODUCTS OF A CATEGORY WHEN THE CATEGORY IS DELETED?
    [HttpDelete]
    public async Task<IActionResult> Delete([Required] string id, CancellationToken cancellationToken)
    {
        await productService.DeleteAsync(id, cancellationToken);
        catalogService.PublishEvent(id);
        return Ok();
    }
}
