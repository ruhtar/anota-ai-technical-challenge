using AnotaAi.Application.Services;
using AnotaAi.Domain.DTOs;
using AnotaAi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AnotaAi.WebAPI.Controllers;

[Route("[controller]")]
//todo: todo endpoint de alteração(post,put,delete) de produto/categoria deve publicar um evento de alteração para alterar o catalogo no S3
public class ProductController : Controller
{
    private readonly IProductService productService;
    private readonly ICatalogService catalogService;

    public ProductController(IProductService productService, ICatalogService catalogService)
    {
        this.productService = productService;
        this.catalogService = catalogService;
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
    public async Task<IActionResult> Create([FromBody] ProductDto productCreateDto)
    {
        var product = new Product(productCreateDto);
        var result = await productService.InsertAsync(product);
        catalogService.PublishEvent([productCreateDto.OwnerId]);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    //todo: make sure that only the required fileds can be passed to the endpoint (PATCH)
    [HttpPatch]
    public async Task<IActionResult> Update([Required] string id, [FromBody] ProductDto productCreateDto)
    {
        var product = new Product(productCreateDto);
        await productService.UpdateAsync(id, product);
        catalogService.PublishEvent([productCreateDto.OwnerId]);
        return Ok();
    }

    //TODO: WHAT SHOULD HAPPEN TO THE PRODUCTS OF A CATEGORY WHEN THE CATEGORY IS DELETED?
    [HttpDelete]
    public async Task<IActionResult> Delete([Required] string id)
    {
        catalogService.PublishEvent([id]);
        await productService.DeleteAsync(id);
        return Ok();
    }
}
