using Microsoft.EntityFrameworkCore;
using HRM.SantaLucia.Web.Data;
using HRM.SantaLucia.Web.Models.Entities;

namespace HRM.SantaLucia.Web.Data.Repositories
{
    public class AsistenciaRepository : IAsistenciaRepository
    {
        private readonly ApplicationDbContext _context;

        public AsistenciaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Asistencia>> GetByFechaAsync(DateTime fecha)
        {
            var fechaKey = int.Parse(fecha.ToString("yyyyMMdd"));

            return await _context.Asistencias
                .Include(a => a.Empleado)
                    .ThenInclude(e => e.Departamento)
                .Where(a => a.FechaKey == fechaKey)
                .OrderBy(a => a.Empleado.NombreCompleto)
                .ToListAsync();
        }

        public async Task<Asistencia> GetByEmpleadoYFechaAsync(int empleadoKey, DateTime fecha)
        {
            var fechaKey = int.Parse(fecha.ToString("yyyyMMdd"));

            return await _context.Asistencias
                .Include(a => a.Empleado)
                .FirstOrDefaultAsync(a => a.EmpleadoKey == empleadoKey && a.FechaKey == fechaKey);
        }

        public async Task<bool> RegistrarAsync(Asistencia asistencia)
        {
            var existing = await GetByEmpleadoYFechaAsync(asistencia.EmpleadoKey,
                DateTime.ParseExact(asistencia.FechaKey.ToString(), "yyyyMMdd", null));

            if (existing != null)
            {
                // Actualizar
                existing.HoraEntrada = asistencia.HoraEntrada ?? existing.HoraEntrada;
                existing.HoraSalida = asistencia.HoraSalida ?? existing.HoraSalida;
                existing.HorasTrabajadas = asistencia.HorasTrabajadas;
                existing.MinutosTarde = asistencia.MinutosTarde;
                existing.Estado = asistencia.Estado;
                existing.Justificacion = asistencia.Justificacion;
                existing.GoceSalario = asistencia.GoceSalario;
                existing.MotivoPermiso = asistencia.MotivoPermiso;
                existing.FechaModificacion = DateTime.Now;
                existing.UsuarioModificacion = asistencia.UsuarioCreacion ?? "SYSTEM";
            }
            else
            {
                // Crear
                _context.Asistencias.Add(asistencia);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Asistencia>> GetByEmpleadoAsync(int empleadoKey, DateTime? desde = null, DateTime? hasta = null)
        {
            var query = _context.Asistencias
                .Include(a => a.Empleado)
                .Where(a => a.EmpleadoKey == empleadoKey);

            if (desde.HasValue)
            {
                var fechaKeyDesde = int.Parse(desde.Value.ToString("yyyyMMdd"));
                query = query.Where(a => a.FechaKey >= fechaKeyDesde);
            }

            if (hasta.HasValue)
            {
                var fechaKeyHasta = int.Parse(hasta.Value.ToString("yyyyMMdd"));
                query = query.Where(a => a.FechaKey <= fechaKeyHasta);
            }

            return await query.OrderByDescending(a => a.FechaKey).ToListAsync();
        }

        public async Task<IEnumerable<Asistencia>> GetByPeriodoAsync(DateTime desde, DateTime hasta)
        {
            var fechaKeyDesde = int.Parse(desde.ToString("yyyyMMdd"));
            var fechaKeyHasta = int.Parse(hasta.ToString("yyyyMMdd"));

            return await _context.Asistencias
                .Include(a => a.Empleado)
                    .ThenInclude(e => e.Departamento)
                .Where(a => a.FechaKey >= fechaKeyDesde && a.FechaKey <= fechaKeyHasta)
                .OrderBy(a => a.FechaKey)
                .ThenBy(a => a.Empleado.NombreCompleto)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetResumenMensualAsync(int ano, int mes)
        {
            var fechaKeyInicio = (ano * 10000) + (mes * 100) + 1;
            var fechaKeyFin = (ano * 10000) + (mes * 100) + 31;

            var resumen = await _context.Asistencias
                .Where(a => a.FechaKey >= fechaKeyInicio && a.FechaKey <= fechaKeyFin)
                .GroupBy(a => a.Estado)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                .ToDictionaryAsync(x => x.Estado, x => x.Cantidad);

            return resumen;
        }
    }
}
