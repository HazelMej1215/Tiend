using System;
using System.Collections.Generic;

namespace TiendaApi.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public DateTime CreadoEn { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
