using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaApi.Models;

namespace TiendaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly TiendaDbContext _db;
    public ProductosController(TiendaDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _db.Productos
            .Select(p => new
            {
                p.IdProducto,
                p.Nombre,
                p.Precio,
                p.Stock,
                p.Sku,
                p.IdCategoria,
                p.IdEstado
            })
            .ToListAsync();

        return Ok(data);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _db.Productos
            .Where(x => x.IdProducto == id)
            .Select(x => new {
                x.IdProducto,
                x.Nombre,
                x.Descripcion,
                x.Precio,
                x.Stock,
                x.Sku,
                x.IdCategoria,
                x.IdEstado
            })
            .FirstOrDefaultAsync();

        if (p == null) return NotFound("Producto no encontrado.");
        return Ok(p);
    }


    [HttpGet("activos")]
    public async Task<IActionResult> GetActivos()
    {
        var data = await _db.Productos
            .Where(p => p.IdEstado == 1 && p.Stock > 0)
            .Select(p => new {
                p.IdProducto,
                p.Nombre,
                p.Precio,
                p.Stock
            })
            .ToListAsync();

        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> PostProducto([FromBody] ProductoCreateDto dto)
    {
        if (dto == null)
            return BadRequest("Datos inválidos.");

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest("El nombre es obligatorio.");

        if (dto.Precio < 0)
            return BadRequest("El precio no puede ser negativo.");

        if (dto.Stock < 0)
            return BadRequest("El stock no puede ser negativo.");

        var producto = new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Stock = dto.Stock,
            Sku = dto.Sku,
            IdCategoria = dto.IdCategoria,
            IdEstado = dto.IdEstado == 0 ? (sbyte)1 : dto.IdEstado,
            CreadoEn = DateTime.Now,
            ActualizadoEn = null
        };

        _db.Productos.Add(producto);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = producto.IdProducto }, new
        {
            message = "Producto agregado",
            producto.IdProducto
        });
    }


}
