using Microsoft.AspNetCore.Mvc;
using ArticulosWeb.Models;
using ArticulosWeb.Services;

namespace ArticulosWeb.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly ArticulosApi _api;

        public ArticulosController(ArticulosApi api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _api.GetAllAsync();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View(new Articulo());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Articulo articulo)
        {
            var (ok, error) = await _api.CreateAsync(articulo);

            if (!ok)
            {
                ViewBag.Error = error;
                return View(articulo);
            }

            TempData["Msg"] = "✅ Producto registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }


    }
}
