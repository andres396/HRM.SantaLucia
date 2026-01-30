using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRM.SantaLucia.Web.Services;
using HRM.SantaLucia.Web.Models.ViewModels;
using HRM.SantaLucia.Web.Data;

namespace HRM.SantaLucia.Web.Controllers
{
    public class RendimientoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RendimientoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rendimiento
        public async Task<IActionResult> Index()
        {
            var rendimientos = await _context.Rendimientos
                .Include(r => r.Empleado)
                .Include(r => r.Evaluador)
                .OrderByDescending(r => r.FechaKey)
                .ToListAsync();

            var model = rendimientos.Select(r => new RendimientoViewModel
            {
                RendimientoKey = r.RendimientoKey,
                EmpleadoKey = r.EmpleadoKey,
                EvaluadorKey = r.EvaluadorKey,
                FechaEvaluacion = DateTime.ParseExact(r.FechaKey.ToString(), "yyyyMMdd", null),
                PeriodoEvaluacion = r.PeriodoEvaluacion,
                CalificacionGeneral = r.CalificacionGeneral,
                MetasPropuestas = r.MetasPropuestas,
                MetasAlcanzadas = r.MetasAlcanzadas,
                Comentarios = r.Comentarios,
                NombreEmpleado = r.Empleado?.NombreCompleto ?? "N/A",
                NombreEvaluador = r.Evaluador?.NombreCompleto ?? "N/A",
                PorcentajeCumplimiento = r.MetasPropuestas > 0 ? (decimal)r.MetasAlcanzadas / r.MetasPropuestas * 100 : 0
            }).ToList();

            return View(model);
        }

        // GET: Rendimiento/Create
        public IActionResult Create()
        {
            LoadViewData();
            return View();
        }

        // POST: Rendimiento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RendimientoViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rendimiento = new Models.Entities.Rendimiento
                    {
                        EmpleadoKey = model.EmpleadoKey,
                        EvaluadorKey = model.EvaluadorKey,
                        FechaKey = int.Parse(model.FechaEvaluacion.ToString("yyyyMMdd")),
                        PeriodoEvaluacion = model.PeriodoEvaluacion,
                        CalificacionGeneral = (model.CalificacionConocimiento + model.CalificacionCalidad + 
                                             model.CalificacionPuntualidad + model.CalificacionTrabajoEquipo + 
                                             model.CalificacionIniciativa) / 5,
                        MetasPropuestas = model.MetasPropuestas,
                        MetasAlcanzadas = model.MetasAlcanzadas,
                        Comentarios = model.Comentarios,
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = User.Identity?.Name ?? "SYSTEM"
                    };

                    _context.Rendimientos.Add(rendimiento);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Evaluación de rendimiento creada exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            LoadViewData();
            return View(model);
        }

        // GET: Rendimiento/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var rendimiento = await _context.Rendimientos
                .Include(r => r.Empleado)
                .Include(r => r.Evaluador)
                .FirstOrDefaultAsync(r => r.RendimientoKey == id);

            if (rendimiento == null)
            {
                return NotFound();
            }

            var model = new RendimientoViewModel
            {
                RendimientoKey = rendimiento.RendimientoKey,
                EmpleadoKey = rendimiento.EmpleadoKey,
                EvaluadorKey = rendimiento.EvaluadorKey,
                FechaEvaluacion = DateTime.ParseExact(rendimiento.FechaKey.ToString(), "yyyyMMdd", null),
                PeriodoEvaluacion = rendimiento.PeriodoEvaluacion,
                CalificacionGeneral = rendimiento.CalificacionGeneral,
                MetasPropuestas = rendimiento.MetasPropuestas,
                MetasAlcanzadas = rendimiento.MetasAlcanzadas,
                Comentarios = rendimiento.Comentarios,
                NombreEmpleado = rendimiento.Empleado?.NombreCompleto ?? "N/A",
                NombreEvaluador = rendimiento.Evaluador?.NombreCompleto ?? "N/A",
                PorcentajeCumplimiento = rendimiento.MetasPropuestas > 0 ? 
                    (decimal)rendimiento.MetasAlcanzadas / rendimiento.MetasPropuestas * 100 : 0
            };

            return View(model);
        }

        private void LoadViewData()
        {
            ViewBag.Empleados = new SelectList(
                _context.Empleados.Where(e => e.Activo).OrderBy(e => e.NombreCompleto),
                "EmpleadoKey",
                "NombreCompleto"
            );
            ViewBag.Evaluadores = new SelectList(
                _context.Empleados.Where(e => e.Activo).OrderBy(e => e.NombreCompleto),
                "EmpleadoKey",
                "NombreCompleto"
            );
        }
    }
}

