using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaApi.Dtos;
using TiendaApi.Models;

namespace TiendaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComprasController : ControllerBase
{
    private readonly TiendaDbContext _db;
    public ComprasController(TiendaDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Comprar([FromBody] CompraRequest req)
    {
        if (req.Items == null || req.Items.Count == 0)
            return BadRequest("Carrito vacío.");

        if (req.Items.Any(i => i.Cantidad <= 0))
            return BadRequest("Cantidad inválida.");

        // Transacción: todo o nada
        using var tx = await _db.Database.BeginTransactionAsync();

        try
        {
            var venta = new Venta
            {
                Fecha = DateTime.Now,
                Total = 0m,            // triggers lo actualizan
                Estatus = "PAGADA",
                IdUsuario = req.IdUsuario
            };

            _db.Ventas.Add(venta);
            await _db.SaveChangesAsync(); // genera IdVenta

            foreach (var item in req.Items)
            {
                var prod = await _db.Productos
                    .FirstOrDefaultAsync(p => p.IdProducto == item.IdProducto);

                if (prod == null)
                    return BadRequest($"No existe el producto: {item.IdProducto}");

                if (prod.IdEstado != 1)
                    return BadRequest($"Producto inactivo: {item.IdProducto}");

                var det = new VentaDetalle
                {
                    IdVenta = venta.IdVenta,
                    IdProducto = prod.IdProducto,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = prod.Precio,
                    Subtotal = 0m // trigger lo recalcula
                };

                _db.VentaDetalles.Add(det);
                await _db.SaveChangesAsync();
            }

            await tx.CommitAsync();

            // leer venta final (total actualizado por trigger)
            var ventaFinal = await _db.Ventas.FirstAsync(v => v.IdVenta == venta.IdVenta);

            return Ok(new
            {
                ventaFinal.IdVenta,
                ventaFinal.Fecha,
                ventaFinal.Total,
                ventaFinal.Estatus
            });
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            // Si hay stock insuficiente, el trigger lanza el error y cae aquí
            return BadRequest(ex.Message);
        }
    }
}
