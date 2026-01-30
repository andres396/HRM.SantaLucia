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
                Mes = DateTime.Now.Month,
                Quincena = DateTime.Now.Day <= 15 ? 1 : 2,
                Sede = "Kamakiri"
            };
            return View(model);
        }

        // POST: Nomina/Calcular
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calcular(int ano, int mes, int quincena, string sede)
        {
            try
            {
                var success = await _nominaService.CalcularNominaAsync(ano, mes, quincena, sede, User.Identity?.Name ?? "SYSTEM");

                if (success)
                {
                    var quincenaTexto = quincena == 1 ? "Primera" : "Segunda";
                    var nombreMes = new DateTime(ano, mes, 1).ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                    TempData["SuccessMessage"] = $"Nómina {quincenaTexto} quincena de {nombreMes} {ano} - {sede} calculada exitosamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al calcular la nómina";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Consultar), new { ano, mes, quincena, sede });
        }

        // GET: Nomina/Consultar
        public async Task<IActionResult> Consultar(int? ano, int? mes, int? quincena, string? sede)
        {
            ano ??= DateTime.Now.Year;
            mes ??= DateTime.Now.Month;
            quincena ??= DateTime.Now.Day <= 15 ? 1 : 2;
            sede ??= "";

            var model = await _nominaService.GetNominaPeriodoAsync(ano.Value, mes.Value, quincena.Value, sede);
            return View(model);
        }

        // GET: Nomina/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var nomina = await _nominaService.GetByIdAsync(id);
            if (nomina == null)
            {
                return NotFound();
            }

            var model = new EditarNominaViewModel
            {
                NominaKey = nomina.NominaKey,
                EmpleadoKey = nomina.EmpleadoKey,
                NombreEmpleado = nomina.NombreEmpleado ?? "",
                SalarioBase = nomina.SalarioBase,
                QTYHorasExtras = nomina.QTYHorasExtras,
                QTYHorasRegulares = nomina.QTYHorasRegulares,
                QTYDiasFeriados = nomina.QTYDiasFeriados,
                Miscelaneo = nomina.Miscelaneo,
                ExtrasQuincenales = nomina.ExtrasQuincenales,
                DeduccionesQuincenales = nomina.DeduccionesQuincenales,
                MesPeriodo = nomina.Mes
            };

            return View(model);
        }

        // POST: Nomina/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditarNominaViewModel model)
        {
            if (model.NominaKey <= 0)
            {
                ModelState.AddModelError("", "Identificador de nómina inválido.");
                return View(model);
            }

            try
            {
                var success = await _nominaService.UpdateNominaAsync(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Nómina actualizada exitosamente";
                    var nomina = await _nominaService.GetByIdAsync(model.NominaKey);
                    if (nomina != null)
                    {
                        return RedirectToAction(nameof(Consultar), new { ano = nomina.Ano, mes = nomina.Mes, quincena = nomina.Dia == 15 ? 1 : 2, sede = nomina.Sede });
                    }
                    return RedirectToAction(nameof(Index));
                }
                TempData["ErrorMessage"] = "No se pudieron guardar los cambios. Verifique los datos.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al guardar: {ex.Message}";
            }

            return View(model);
        }

        // GET: Nomina/ExportarExcel
        public async Task<IActionResult> ExportarExcel(int ano, int mes, int quincena, string? sede)
        {
            try
            {
                var excelBytes = await _nominaService.ExportarNominaExcelAsync(ano, mes, quincena, sede);
                var quincenaTexto = quincena == 1 ? "Primera" : "Segunda";
                var nombreMes = new DateTime(ano, mes, 1).ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                var sedeTexto = sede ?? "Todas";
                var fileName = $"Nomina_{quincenaTexto}Quincena_{nombreMes}_{ano}_{sedeTexto}.xlsx";

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al exportar: {ex.Message}";
                return RedirectToAction(nameof(Consultar), new { ano, mes, quincena, sede });
            }
        }

        // GET: Nomina/Historial/5
        public async Task<IActionResult> Historial(int id)
        {
            var historial = await _nominaService.GetHistorialEmpleadoAsync(id);
            return View(historial);
        }
    }
}
