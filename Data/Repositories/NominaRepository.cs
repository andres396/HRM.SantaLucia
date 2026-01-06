using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using HRM.SantaLucia.Web.Data;
using HRM.SantaLucia.Web.Models.Entities;

namespace HRM.SantaLucia.Web.Data.Repositories
{
    public class NominaRepository : INominaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public NominaRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            // Usar la cadena de conexión directamente sin modificaciones
            // El DbContext ya tiene la cadena configurada correctamente
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Nomina>> GetByPeriodoAsync(int ano, int mes)
        {
            var periodoKey = ano * 100 + mes;

            return await _context.Nominas
                .Include(n => n.Empleado)
                .Include(n => n.Puesto)
                .Include(n => n.Departamento)
                .Where(n => n.PeriodoKey == periodoKey)
                .OrderBy(n => n.Empleado.NombreCompleto)
                .ToListAsync();
        }

        public async Task<Nomina> GetByEmpleadoYPeriodoAsync(int empleadoKey, int ano, int mes)
        {
            var periodoKey = ano * 100 + mes;

            return await _context.Nominas
                .Include(n => n.Empleado)
                .Include(n => n.Puesto)
                .Include(n => n.Departamento)
                .FirstOrDefaultAsync(n => n.EmpleadoKey == empleadoKey && n.PeriodoKey == periodoKey);
        }

        public async Task<bool> CalcularNominaAsync(int ano, int mes, string usuarioCreacion)
        {
            // Implementación simplificada - calcular nómina básica
            try
            {
                var periodoKey = ano * 100 + mes;
                var empleados = await _context.Empleados
                    .Include(e => e.Puesto)
                    .Include(e => e.Departamento)
                    .Where(e => e.Activo && e.FechaIngreso <= new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes)))
                    .ToListAsync();

                foreach (var empleado in empleados)
                {
                    // Validar que el empleado tenga PuestoKey y DepartamentoKey asignados
                    if (!empleado.PuestoKey.HasValue || !empleado.DepartamentoKey.HasValue)
                    {
                        // Saltar empleados sin puesto o departamento asignado
                        continue;
                    }

                    var existing = await _context.Nominas
                        .FirstOrDefaultAsync(n => n.EmpleadoKey == empleado.EmpleadoKey && n.PeriodoKey == periodoKey);

                    if (existing == null)
                    {
                        var salarioBase = empleado.Puesto?.SalarioMinimo ?? 0;
                        var nomina = new Nomina
                        {
                            EmpleadoKey = empleado.EmpleadoKey,
                            PuestoKey = empleado.PuestoKey.Value,
                            DepartamentoKey = empleado.DepartamentoKey.Value,
                            PeriodoKey = periodoKey,
                            SalarioBase = salarioBase,
                            HorasTrabajadas = 160, // Asumiendo 160 horas mensuales
                            TotalExtras = 0,
                            TotalDeducciones = salarioBase * 0.10m, // 10% deducciones
                            SalarioNeto = salarioBase * 0.90m,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = usuarioCreacion
                        };

                        _context.Nominas.Add(nomina);
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<decimal> GetTotalNominaAsync(int ano, int mes)
        {
            var periodoKey = ano * 100 + mes;

            return await _context.Nominas
                .Where(n => n.PeriodoKey == periodoKey)
                .SumAsync(n => n.SalarioNeto);
        }

        public async Task<IEnumerable<Nomina>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12)
        {
            return await _context.Nominas
                .Include(n => n.Puesto)
                .Include(n => n.Departamento)
                .Where(n => n.EmpleadoKey == empleadoKey)
                .OrderByDescending(n => n.PeriodoKey)
                .Take(top)
                .ToListAsync();
        }
    }
}
