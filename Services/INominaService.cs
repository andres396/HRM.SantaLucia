using HRM.SantaLucia.Web.Models.ViewModels;

namespace HRM.SantaLucia.Web.Services
{
    public interface INominaService
    {
        Task<CalcularNominaViewModel> GetNominaPeriodoAsync(int ano, int mes);
        Task<bool> CalcularNominaAsync(int ano, int mes, string usuarioCreacion);
        Task<IEnumerable<NominaViewModel>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12);
        Task<decimal> GetTotalNominaAsync(int ano, int mes);
    }
}
