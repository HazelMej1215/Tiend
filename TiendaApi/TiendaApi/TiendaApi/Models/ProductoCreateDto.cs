namespace TiendaApi.Models
{
    public class ProductoCreateDto
    {
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Sku { get; set; }
        public int IdCategoria { get; set; }
        public sbyte IdEstado { get; set; } = 1;
    }
}
