using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaApi.Models;

namespace TiendaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VentasController : ControllerBase
{
    private readonly TiendaDbContext _db;
    public VentasController(TiendaDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var ventas = await _db.Ventas
            .OrderByDescending(v => v.IdVenta)
            .Select(v => new
            {
                v.IdVenta,
                v.Fecha,
                v.Total,
                v.Estatus,
                v.IdUsuario
            })
            .ToListAsync();

        return Ok(ventas);
    }

    [HttpGet("{id:int}/detalle")]
    public async Task<IActionResult> Detalle(int id)
    {
        var existe = await _db.Ventas.AnyAsync(v => v.IdVenta == id);
        if (!existe) return NotFound("Venta no encontrada.");

        var detalle = await _db.VentaDetalles
            .Where(d => d.IdVenta == id)
            .Join(_db.Productos,
                d => d.IdProducto,
                p => p.IdProducto,
                (d, p) => new
                {
                    d.IdDetalle,
                    d.IdProducto,
                    Producto = p.Nombre,
                    d.Cantidad,
                    d.PrecioUnitario,
                    d.Subtotal
                })
            .ToListAsync();

        return Ok(detalle);
    }
}
