using Microsoft.EntityFrameworkCore;
using HRM.SantaLucia.Web.Data;
using HRM.SantaLucia.Web.Models.ViewModels;

namespace HRM.SantaLucia.Web.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var totalEmpleadosActivos = await _context.Empleados.CountAsync(e => e.Activo);
            var totalDocentes = await _context.Empleados
                .Include(e => e.Puesto)
                .CountAsync(e => e.Activo && e.Puesto != null && e.Puesto.TipoPuesto == "Docente");
            var totalAdministrativos = await _context.Empleados
                .Include(e => e.Puesto)
                .CountAsync(e => e.Activo && e.Puesto != null && e.Puesto.TipoPuesto == "Administrativo");

            var anoActual = DateTime.Now.Year;
            var mesActual = DateTime.Now.Month;
            var periodoKey = anoActual * 100 + mesActual;

            var nominasMes = await _context.Nominas
                .Where(n => n.PeriodoKey == periodoKey)
                .ToListAsync();

            var totalNominaMes = nominasMes.Sum(n => n.SalarioNeto);
            var promedioSalario = nominasMes.Any() ? nominasMes.Average(n => n.SalarioNeto) : 0;

            var vacacionesPendientes = await _context.Vacaciones
                .CountAsync(v => v.Estado == "Pendiente");

            var fechaHoy = DateTime.Today;
            var fechaKey = int.Parse(fechaHoy.ToString("yyyyMMdd"));
            var asistenciaHoy = await _context.Asistencias
                .CountAsync(a => a.FechaKey == fechaKey);

            return new DashboardViewModel
            {
                TotalEmpleadosActivos = totalEmpleadosActivos,
                TotalDocentes = totalDocentes,
                TotalAdministrativos = totalAdministrativos,
                TotalNominaMes = totalNominaMes,
                PromedioSalario = promedioSalario,
                VacacionesPendientes = vacacionesPendientes,
                AsistenciaHoy = asistenciaHoy,
                ProximosCumpleanos = new List<ProximoCumpleanosDto>(),
                ResumenNominaMensual = new List<NominaResumenDto>()
            };
        }
    }
}