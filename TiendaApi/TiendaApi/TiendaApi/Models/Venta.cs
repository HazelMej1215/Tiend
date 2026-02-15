using System;
using System.Collections.Generic;

namespace TiendaApi.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Total { get; set; }

    public string Estatus { get; set; } = null!;

    public int? IdUsuario { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
