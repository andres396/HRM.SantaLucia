using HRM.SantaLucia.Web.Models.Entities;

namespace HRM.SantaLucia.Web.Data.Repositories
{
    public interface INominaRepository
    {
        Task<IEnumerable<Nomina>> GetByPeriodoAsync(int ano, int mes);
        Task<Nomina> GetByEmpleadoYPeriodoAsync(int empleadoKey, int ano, int mes);
        Task<bool> CalcularNominaAsync(int ano, int mes, string usuarioCreacion);
        Task<decimal> GetTotalNominaAsync(int ano, int mes);
        Task<IEnumerable<Nomina>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12);
    }
}
