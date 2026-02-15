using System;
using System.Collections.Generic;

namespace TiendaApi.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public string? Sku { get; set; }

    public int IdCategoria { get; set; }

    public sbyte IdEstado { get; set; }

    public DateTime CreadoEn { get; set; }

    public DateTime? ActualizadoEn { get; set; }

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual EstadosProducto IdEstadoNavigation { get; set; } = null!;

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
