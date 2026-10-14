using Microsoft.AspNetCore.Mvc;
using Products.API.Models;
using Products.Application.Ports;
using Products.Domain.Entities;

namespace Products.API.Controllers;

[ApiController]

// Ruta base para todos los endpoints
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository) => _repository = repository;

    // POST /api/products
    // Crear un nuevo producto
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request)
    {
        var product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.InitialStock
        );

        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // GET /api/products/{id}
    // Buscar un producto por su identificador
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    // GET /api/products/?page=1&pageSize=10
    // Obtener productos paginados
    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var (items, totalCount) = await _repository.GetPagedAsync(page, pageSize);

        return Ok(new
        {
            data = items,
            pagination = new
            {
                currentPage = page,
                pageSize,
                totalCount,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        });
    }

    // PATCH /api/products/{id}
    // Actualizar el inventario de un producto
    [HttpPatch("{id:guid}/stock")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdjustStock(
        Guid id, [FromBody]
        AdjustStockRequest request)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return NotFound();

        try
        {
            product.AdjustStock(request.Quantity);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}