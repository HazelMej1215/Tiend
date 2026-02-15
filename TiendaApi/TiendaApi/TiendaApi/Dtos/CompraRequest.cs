namespace TiendaApi.Dtos;

public class CompraRequest
{
    public int? IdUsuario { get; set; }
    public List<ItemCompra> Items { get; set; } = new();
}

public class ItemCompra
{
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
}
