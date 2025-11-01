namespace HRM.SantaLucia.Web.Controllers
{
    public class AsistenciaController : Controller
    {
        private readonly IAsistenciaService _asistenciaService;
        private readonly ApplicationDbContext _context;

        public AsistenciaController(IAsistenciaService asistenciaService, ApplicationDbContext context)
        {
            _asistenciaService = asistenciaService;
            _context = context;
        }

        // GET: Asistencia
        public async Task<IActionResult> Index(DateTime? fecha)
        {
            fecha ??= DateTime.Today;
            var model = await _asistenciaService.GetAsistenciaDiariaAsync(fecha.Value);
            return View(model);
        }

        // GET: Asistencia/Registrar
        public IActionResult Registrar(DateTime? fecha)
        {
            var model = new AsistenciaViewModel
            {
                Fecha = fecha ?? DateTime.Today,
                Estado = "Presente"
            };

            LoadViewData();
            return View(model);
        }

        // POST: Asistencia/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(AsistenciaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _asistenciaService.RegistrarAsistenciaAsync(model);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Asistencia registrada exitosamente";
                        return RedirectToAction(nameof(Index), new { fecha = model.Fecha });
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo registrar la asistencia");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            LoadViewData();
            return View(model);
        }

        // GET: Asistencia/Historial/5
        public async Task<IActionResult> Historial(int id, DateTime? desde, DateTime? hasta)
        {
            desde ??= DateTime.Today.AddMonths(-1);
            hasta ??= DateTime.Today;

            var historial = await _asistenciaService.GetHistorialEmpleadoAsync(id, desde, hasta);

            ViewBag.EmpleadoKey = id;
            ViewBag.Desde = desde;
            ViewBag.Hasta = hasta;

            return View(historial);
        }

        // GET: Asistencia/Resumen
        public async Task<IActionResult> Resumen(int? año, int? mes)
        {
            año ??= DateTime.Now.Year;
            mes ??= DateTime.Now.Month;

            var resumen = await _asistenciaService.GetResumenMensualAsync(año.Value, mes.Value);

            ViewBag.Año = año;
            ViewBag.Mes = mes;
            ViewBag.NombreMes = new DateTime(año.Value, mes.Value, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-ES"));

            return View(resumen);
        }

        private void LoadViewData()
        {
            ViewBag.Empleados = new SelectList(
                _context.Empleados.Where(e => e.Activo).OrderBy(e => e.NombreCompleto),
                "EmpleadoKey",
                "NombreCompleto"
            );
            ViewBag.Estados = new SelectList(new[] { "Presente", "Tarde", "Ausente", "Justificado", "Permiso" });
        }
    }
}