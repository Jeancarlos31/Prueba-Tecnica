using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductosApi.Data;
using ProductosApi.DTOs;
using ProductosApi.Models;

namespace ProductosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Products
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .ToListAsync();

        return Ok(products.Select(ToDto));
    }

    // GET: api/Products/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
        {
            return NotFound(new { message = $"No se encontró el producto con Id {id}." });
        }

        return Ok(ToDto(product));
    }

    // POST: api/Products
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> CreateProduct(ProductInputDto dto)
    {
        // [ApiController] valida automáticamente el ModelState y responde
        // 400 Bad Request con los detalles si el DTO no cumple las anotaciones.

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, ToDto(product));
    }

    // PUT: api/Products/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(int id, ProductInputDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound(new { message = $"No se encontró el producto con Id {id}." });
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Products/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound(new { message = $"No se encontró el producto con Id {id}." });
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static ProductDto ToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        Stock = p.Stock,
        CreatedAt = p.CreatedAt
    };
}
