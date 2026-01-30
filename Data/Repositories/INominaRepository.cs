using HRM.SantaLucia.Web.Models.Entities;

namespace HRM.SantaLucia.Web.Data.Repositories
{
    public interface INominaRepository
    {
        Task<IEnumerable<Nomina>> GetByPeriodoAsync(int ano, int mes, int quincena, string? sede);
        Task<Nomina> GetByEmpleadoYPeriodoAsync(int empleadoKey, int periodoKey);
        Task<Nomina?> GetByIdAsync(int nominaKey);
        Task<bool> CalcularNominaAsync(int ano, int mes, int quincena, string sede, string usuarioCreacion);
        Task<bool> UpdateAsync(Nomina nomina);
        Task<decimal> GetTotalNominaAsync(int ano, int mes, int quincena, string? sede);
        Task<IEnumerable<Nomina>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12);
    }
}
