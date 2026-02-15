using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ArticulosWeb.Models;

namespace ArticulosWeb.Services
{
    public class ArticulosApi
    {
        private readonly HttpClient _http;

        public ArticulosApi(HttpClient http)
        {
            _http = http;
        }

        // LISTAR
        public async Task<List<Articulo>> GetAllAsync()
        {
            var resp = await _http.GetAsync("/api/Productos");
            if (!resp.IsSuccessStatusCode) return new List<Articulo>();

            var data = await resp.Content.ReadFromJsonAsync<List<Articulo>>();
            return data ?? new List<Articulo>();
        }

        // CREAR (y regresar error detallado)
        public async Task<(bool ok, string error)> CreateAsync(Articulo articulo)
        {
            var payload = new
            {
                nombre = articulo.Nombre,
                descripcion = articulo.Descripcion,
                precio = articulo.Precio,
                stock = articulo.Stock,
                sku = articulo.Sku,
                idCategoria = articulo.IdCategoria,   // 👈 camelCase
                idEstado = articulo.IdEstado          // 👈 camelCase
            };


            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _http.PostAsync("/api/Productos", content);
            var body = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode) return (true, "");

            return (false, $"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}\n{body}");
        }
    }
}
