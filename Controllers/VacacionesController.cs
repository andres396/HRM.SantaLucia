using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRM.SantaLucia.Web.Services;
using HRM.SantaLucia.Web.Models.ViewModels;
using HRM.SantaLucia.Web.Data;

namespace HRM.SantaLucia.Web.Controllers
{
    public class VacacionesController : Controller
    {
        private readonly IVacacionesService _vacacionesService;
        private readonly ApplicationDbContext _context;

        public VacacionesController(IVacacionesService vacacionesService, ApplicationDbContext context)
        {
            _vacacionesService = vacacionesService;
            _context = context;
        }

        // GET: Vacaciones
        public async Task<IActionResult> Index()
        {
            var model = await _vacacionesService.GetPendientesAsync();
            return View(model);
        }

        // GET: Vacaciones/Solicitar
        public async Task<IActionResult> Solicitar(int? empleadoKey)
        {
            var model = new VacacionesViewModel
            {
                EmpleadoKey = empleadoKey ?? 0,
                FechaInicio = DateTime.Today.AddDays(1),
                Estado = "Pendiente"
            };

            if (empleadoKey.HasValue)
            {
                model.DiasDisponibles = await _vacacionesService.GetDiasDisponiblesAsync(empleadoKey.Value);
            }

            LoadViewData();
            return View(model);
        }

        // POST: Vacaciones/Solicitar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Solicitar(VacacionesViewModel model)
        {
            if (model.FechaInicio != default && model.FechaFin != default && model.FechaFin > model.FechaInicio)
            {
                model.DiasSolicitados = (model.FechaFin - model.FechaInicio).Days + 1;
                ModelState.Remove("DiasSolicitados");
            }
            if (model.EmpleadoKey <= 0)
                ModelState.AddModelError("EmpleadoKey", "Debe seleccionar un empleado");
            if (model.FechaFin <= model.FechaInicio && model.FechaInicio != default)
                ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la fecha de inicio");

            if (ModelState.IsValid)
            {
                try
                {
                    // Validar fechas
                    if (model.FechaFin <= model.FechaInicio)
                    {
                        ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la fecha de inicio");
                        LoadViewData();
                        return View(model);
                    }

                    // Calcular d�as solicitados
                    model.DiasSolicitados = (model.FechaFin - model.FechaInicio).Days + 1;

                    if (model.DiasSolicitados < 1)
                    {
                        ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la fecha de inicio");
                        LoadViewData();
                        return View(model);
                    }

                    model.Estado = "Pendiente";
                    var vacacionKey = await _vacacionesService.SolicitarAsync(model);
                    TempData["SuccessMessage"] = "Solicitud de vacaciones enviada exitosamente";
                    return RedirectToAction(nameof(MisSolicitudes), new { empleadoKey = model.EmpleadoKey });
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            if (model.EmpleadoKey > 0)
                model.DiasDisponibles = await _vacacionesService.GetDiasDisponiblesAsync(model.EmpleadoKey);
            LoadViewData();
            return View(model);
        }

        // GET: Vacaciones/MisSolicitudes/5
        public async Task<IActionResult> MisSolicitudes(int empleadoKey)
        {
            var solicitudes = await _vacacionesService.GetByEmpleadoAsync(empleadoKey);
            ViewBag.EmpleadoKey = empleadoKey;
            ViewBag.DiasDisponibles = await _vacacionesService.GetDiasDisponiblesAsync(empleadoKey);
            return View(solicitudes);
        }

        // POST: Vacaciones/Aprobar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar(int id, string observaciones)
        {
            try
            {
                var aprobadorKey = await _context.Empleados
                    .Where(e => e.Activo)
                    .OrderBy(e => e.EmpleadoKey)
                    .Select(e => e.EmpleadoKey)
                    .FirstOrDefaultAsync();

                var success = await _vacacionesService.AprobarAsync(id, aprobadorKey, observaciones);
                if (success)
                {
                    TempData["SuccessMessage"] = "Vacaciones aprobadas exitosamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudieron aprobar las vacaciones";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Vacaciones/Rechazar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rechazar(int id, string observaciones)
        {
            try
            {
                var aprobadorKey = await _context.Empleados
                    .Where(e => e.Activo)
                    .OrderBy(e => e.EmpleadoKey)
                    .Select(e => e.EmpleadoKey)
                    .FirstOrDefaultAsync();

                var success = await _vacacionesService.RechazarAsync(id, aprobadorKey, observaciones);
                if (success)
                {
                    TempData["SuccessMessage"] = "Vacaciones rechazadas";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudieron rechazar las vacaciones";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadViewData()
        {
            ViewBag.Empleados = new SelectList(
                _context.Empleados.Where(e => e.Activo).OrderBy(e => e.NombreCompleto),
                "EmpleadoKey",
                "NombreCompleto"
            );
        }
    }
}