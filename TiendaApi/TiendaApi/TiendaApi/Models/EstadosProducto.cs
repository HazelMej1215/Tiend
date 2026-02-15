using System;
using System.Collections.Generic;

namespace TiendaApi.Models;

public partial class EstadosProducto
{
    public sbyte IdEstado { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
