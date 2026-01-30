using HRM.SantaLucia.Web.Models.ViewModels;

namespace HRM.SantaLucia.Web.Services
{
    public interface INominaService
    {
        Task<CalcularNominaViewModel> GetNominaPeriodoAsync(int ano, int mes, int quincena, string? sede);
        Task<bool> CalcularNominaAsync(int ano, int mes, int quincena, string sede, string usuarioCreacion);
        Task<NominaViewModel?> GetByIdAsync(int nominaKey);
        Task<bool> UpdateNominaAsync(EditarNominaViewModel model);
        Task<byte[]> ExportarNominaExcelAsync(int ano, int mes, int quincena, string? sede);
        Task<IEnumerable<NominaViewModel>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12);
        Task<decimal> GetTotalNominaAsync(int ano, int mes, int quincena, string? sede);
    }
}
