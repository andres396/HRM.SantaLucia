using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HRM.SantaLucia.Web.Services;
using HRM.SantaLucia.Web.Models.ViewModels;
using HRM.SantaLucia.Web.Data;

namespace HRM.SantaLucia.Web.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly ApplicationDbContext _context;

        public EmpleadoController(IEmpleadoService empleadoService, ApplicationDbContext context)
        {
            _empleadoService = empleadoService;
            _context = context;
        }

        // GET: Empleado
        public async Task<IActionResult> Index(string searchTerm, int? departamentoFilter, bool soloActivos = true)
        {
            IEnumerable<EmpleadoViewModel> empleados;

            if (!string.IsNullOrWhiteSpace(searchTerm) || departamentoFilter.HasValue)
            {
                empleados = await _empleadoService.SearchAsync(searchTerm, departamentoFilter, soloActivos);
            }
            else
            {
                empleados = await _empleadoService.GetAllAsync(soloActivos);
            }

            var model = new EmpleadoListViewModel
            {
                Empleados = empleados.ToList(),
                SearchTerm = searchTerm,
                DepartamentoFilter = departamentoFilter,
                SoloActivos = soloActivos,
                TotalRegistros = empleados.Count()
            };

            // Cargar lista de departamentos para el filtro
            ViewBag.Departamentos = new SelectList(_context.Departamentos.Where(d => d.Activo), "DepartamentoKey", "NombreDepartamento");

            return View(model);
        }

        // GET: Empleado/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var empleado = await _empleadoService.GetByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleado/Create
        public IActionResult Create()
        {
            LoadViewData();
            return View();
        }

        // POST: Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmpleadoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existentePorCedula = await _empleadoService.GetByCedulaAsync(model.Cedula);
                if (existentePorCedula != null)
                {
                    TempData["ErrorMessage"] = "La cédula ya existe. Se abrió el registro existente en modo edición para que actualices los datos.";
                    return RedirectToAction(nameof(Edit), new { id = existentePorCedula.EmpleadoKey });
                }

                var existentePorEmpleadoId = await _empleadoService.GetByEmpleadoIdAsync(model.EmpleadoID);
                if (existentePorEmpleadoId != null)
                {
                    TempData["ErrorMessage"] = "El código de empleado ya existe. Se abrió el registro existente en modo edición para que actualices los datos.";
                    return RedirectToAction(nameof(Edit), new { id = existentePorEmpleadoId.EmpleadoKey });
                }

                // Validar c�dula �nica
                if (!await _empleadoService.ValidarCedulaUnicaAsync(model.Cedula))
                {
                    ModelState.AddModelError("Cedula", "Ya existe un empleado con esta c�dula");
                    LoadViewData();
                    return View(model);
                }

                // Validar email �nico
                if (!await _empleadoService.ValidarEmailUnicoAsync(model.Email))
                {
                    ModelState.AddModelError("Email", "Ya existe un empleado con este correo electr�nico");
                    LoadViewData();
                    return View(model);
                }

                try
                {
                    var empleadoKey = await _empleadoService.CreateAsync(model, User.Identity?.Name ?? "SYSTEM");
                    TempData["SuccessMessage"] = "Empleado creado exitosamente";
                    return RedirectToAction(nameof(Details), new { id = empleadoKey });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al crear el empleado: {ex.Message}");
                }
            }

            LoadViewData();
            return View(model);
        }

        // GET: Empleado/Edit/5
        public async Task<IActionResult> Edit(int id, bool modal = false)
        {
            var empleado = await _empleadoService.GetByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            ViewBag.IsModal = modal;
            LoadViewData();
            return View(empleado);
        }

        // POST: Empleado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmpleadoViewModel model, bool modal = false)
        {
            if (model.EmpleadoKey <= 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Validar c�dula �nica (excluyendo el empleado actual)
                if (!await _empleadoService.ValidarCedulaUnicaAsync(model.Cedula, model.EmpleadoKey))
                {
                    ModelState.AddModelError("Cedula", "Ya existe un empleado con esta c�dula");
                    LoadViewData();
                    return View(model);
                }

                // Validar email �nico (excluyendo el empleado actual)
                if (!await _empleadoService.ValidarEmailUnicoAsync(model.Email, model.EmpleadoKey))
                {
                    ModelState.AddModelError("Email", "Ya existe un empleado con este correo electr�nico");
                    LoadViewData();
                    return View(model);
                }

                try
                {
                    var success = await _empleadoService.UpdateAsync(model);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Empleado actualizado exitosamente";
                        if (modal)
                        {
                            return Content("<script>window.parent.location.reload();</script>", "text/html");
                        }
                        return RedirectToAction(nameof(Details), new { id = model.EmpleadoKey });
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo actualizar el empleado");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al actualizar el empleado: {ex.Message}");
                }
            }

            LoadViewData();
            return View(model);
        }

        // GET: Empleado/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await _empleadoService.GetByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _empleadoService.DeleteAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Empleado dado de baja exitosamente";
                    return RedirectToAction(nameof(Details), new { id });
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo dar de baja al empleado";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al dar de baja al empleado: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Empleado/Activar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activar(int id)
        {
            try
            {
                var empleado = await _empleadoService.GetByIdAsync(id);
                if (empleado == null)
                {
                    return NotFound();
                }

                empleado.Activo = true;
                empleado.FechaSalida = null;
                var success = await _empleadoService.UpdateAsync(empleado);
                
                if (success)
                {
                    TempData["SuccessMessage"] = "Empleado reactivado exitosamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo reactivar al empleado";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al reactivar al empleado: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        private void LoadViewData()
        {
            ViewBag.Puestos = new SelectList(_context.Puestos.Where(p => p.Activo).OrderBy(p => p.NombrePuesto), "PuestoKey", "NombrePuesto");
            ViewBag.Departamentos = new SelectList(_context.Departamentos.Where(d => d.Activo).OrderBy(d => d.NombreDepartamento), "DepartamentoKey", "NombreDepartamento");
            ViewBag.Bancos = new SelectList(_context.Bancos.Where(b => b.Activo).OrderBy(b => b.NombreBanco), "BancoKey", "NombreBanco");

            ViewBag.Generos = new SelectList(new[] { "Masculino", "Femenino", "Otro" });
            ViewBag.EstadosCiviles = new SelectList(new[] { "Soltero", "Casado", "Divorciado", "Viudo", "Union Libre" });
            ViewBag.TiposContrato = new SelectList(new[] { "Indefinido", "Plazo Fijo", "Por Servicios" });
            ViewBag.Provincias = new SelectList(new[] { "San Jose", "Alajuela", "Cartago", "Heredia", "Guanacaste", "Puntarenas", "Limon" });
            ViewBag.Sedes = new SelectList(new[] { "Kamakiri", "Complejo Educativo" });
        }
    }
}