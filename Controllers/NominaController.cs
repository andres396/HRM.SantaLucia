using Microsoft.AspNetCore.Mvc;
using HRM.SantaLucia.Web.Services;
using HRM.SantaLucia.Web.Models.ViewModels;

namespace HRM.SantaLucia.Web.Controllers
{
    public class NominaController : Controller
    {
        private readonly INominaService _nominaService;

        public NominaController(INominaService nominaService)
        {
            _nominaService = nominaService;
        }

        // GET: Nomina
        public IActionResult Index()
        {
            var model = new CalcularNominaViewModel
            {
                Ano = DateTime.Now.Year,
                Mes = DateTime.Now.Month
            };
            return View(model);
        }

        // POST: Nomina/Calcular
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calcular(int ano, int mes)
        {
            try
            {
                var success = await _nominaService.CalcularNominaAsync(ano, mes, User.Identity?.Name ?? "SYSTEM");

                if (success)
                {
                    TempData["SuccessMessage"] = $"Nomina de {new DateTime(ano, mes, 1):MMMM yyyy} calculada exitosamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al calcular la nomina";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Consultar), new { ano, mes });
        }

        // GET: Nomina/Consultar
        public async Task<IActionResult> Consultar(int? ano, int? mes)
        {
            ano ??= DateTime.Now.Year;
            mes ??= DateTime.Now.Month;

            var model = await _nominaService.GetNominaPeriodoAsync(ano.Value, mes.Value);
            return View(model);
        }

        // GET: Nomina/Historial/5
        public async Task<IActionResult> Historial(int id)
        {
            var historial = await _nominaService.GetHistorialEmpleadoAsync(id);
            return View(historial);
        }
    }
}
