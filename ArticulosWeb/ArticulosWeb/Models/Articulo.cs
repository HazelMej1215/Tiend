using System.Text.Json.Serialization;

namespace ArticulosWeb.Models
{
    public class Articulo
    {
        [JsonPropertyName("idProducto")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = "";

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = "";

        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [JsonPropertyName("sku")]
        public string Sku { get; set; } = "";

        [JsonPropertyName("id_categoria")]
        public int IdCategoria { get; set; }

        [JsonPropertyName("id_estado")]
        public int IdEstado { get; set; } = 1;
    }
}
