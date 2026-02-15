using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaApi.Models;

namespace TiendaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesController : ControllerBase
{
    private readonly TiendaDbContext _db;
    public ReportesController(TiendaDbContext db) => _db = db;

    [HttpGet("ventas-hoy")]
    public async Task<IActionResult> VentasHoy()
    {
        var hoy = DateTime.Today;
        var maniana = hoy.AddDays(1);

        var total = await _db.Ventas
            .Where(v => v.Fecha >= hoy && v.Fecha < maniana && v.Estatus == "PAGADA")
            .SumAsync(v => (decimal?)v.Total) ?? 0m;

        var count = await _db.Ventas
            .CountAsync(v => v.Fecha >= hoy && v.Fecha < maniana && v.Estatus == "PAGADA");

        return Ok(new { ventas = count, total });
    }

    [HttpGet("top-productos")]
    public async Task<IActionResult> TopProductos()
    {
        var top = await _db.VentaDetalles
            .Join(_db.Productos,
                d => d.IdProducto,
                p => p.IdProducto,
                (d, p) => new { d, p })
            .GroupBy(x => new { x.p.IdProducto, x.p.Nombre })
            .Select(g => new {
                g.Key.IdProducto,
                g.Key.Nombre,
                Vendidos = g.Sum(x => x.d.Cantidad)
            })
            .OrderByDescending(x => x.Vendidos)
            .Take(5)
            .ToListAsync();

        return Ok(top);
    }
}
