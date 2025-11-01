using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HRM.SantaLucia.Web.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly string _connectionString;

        public DashboardService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync(
                    "dbo.USP_Dashboard_Principal",
                    commandType: CommandType.StoredProcedure))
                {
                    // Leer resultados
                    var empleadosData = await multi.ReadFirstOrDefaultAsync<dynamic>();
                    var nominaData = await multi.ReadFirstOrDefaultAsync<dynamic>();
                    var vacacionesData = await multi.ReadFirstOrDefaultAsync<dynamic>();
                    var asistenciaData = await multi.ReadFirstOrDefaultAsync<dynamic>();

                    return new DashboardViewModel
                    {
                        TotalEmpleadosActivos = empleadosData?.TotalEmpleadosActivos ?? 0,
                        TotalDocentes = empleadosData?.TotalDocentes ?? 0,
                        TotalAdministrativos = empleadosData?.TotalAdministrativos ?? 0,
                        TotalNominaMes = nominaData?.TotalPagar ?? 0,
                        PromedioSalario = nominaData?.PromedioSalario ?? 0,
                        VacacionesPendientes = vacacionesData?.VacacionesPendientes ?? 0,
                        AsistenciaHoy = asistenciaData?.TotalRegistros ?? 0
                    };
                }
            }
        }
    }
}