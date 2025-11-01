using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HRM.SantaLucia.Web.Data.Repositories
{
    public class NominaRepository : INominaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public NominaRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Nomina>> GetByPeriodoAsync(int año, int mes)
        {
            var fechaKey = (año * 10000) + (mes * 100) + 1;

            return await _context.Nominas
                .Include(n => n.Empleado)
                .Include(n => n.Puesto)
                .Include(n => n.Departamento)
                .Where(n => n.FechaKey == fechaKey)
                .OrderBy(n => n.Empleado.NombreCompleto)
                .ToListAsync();
        }

        public async Task<Nomina> GetByEmpleadoYPeriodoAsync(int empleadoKey, int año, int mes)
        {
            var fechaKey = (año * 10000) + (mes * 100) + 1;

            return await _context.Nominas
                .Include(n => n.Empleado)
                .Include(n => n.Puesto)
                .Include(n => n.Departamento)
                .FirstOrDefaultAsync(n => n.EmpleadoKey == empleadoKey && n.FechaKey == fechaKey);
        }

        public async Task<bool> CalcularNominaAsync(int año, int mes, string usuarioCreacion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Año", año);
                    parameters.Add("@Mes", mes);
                    parameters.Add("@UsuarioCreacion", usuarioCreacion);

                    await connection.ExecuteAsync(
                        "FACT.USP_Calcular_Nomina",
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 120
                    );

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<decimal> GetTotalNominaAsync(int año, int mes)
        {
            var fechaKey = (año * 10000) + (mes * 100) + 1;

            return await _context.Nominas
                .Where(n => n.FechaKey == fechaKey)
                .SumAsync(n => n.SalarioNeto);
        }

        public async Task<IEnumerable<Nomina>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12)
        {
            return await _context.Nominas
                .Include(n => n.Puesto)
                .Include(n => n.Departamento)
                .Where(n => n.EmpleadoKey == empleadoKey)
                .OrderByDescending(n => n.FechaKey)
                .Take(top)
                .ToListAsync();
        }
    }
}