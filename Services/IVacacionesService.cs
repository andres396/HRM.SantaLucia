using HRM.SantaLucia.Web.Models.ViewModels;

namespace HRM.SantaLucia.Web.Services
{
    public interface IVacacionesService
    {
        Task<VacacionesPendientesViewModel> GetPendientesAsync();
        Task<IEnumerable<VacacionesViewModel>> GetByEmpleadoAsync(int empleadoKey);
        Task<int> SolicitarAsync(VacacionesViewModel model);
        Task<bool> AprobarAsync(int vacacionKey, int aprobadorKey, string observaciones);
        Task<bool> RechazarAsync(int vacacionKey, int aprobadorKey, string observaciones);
        Task<int> GetDiasDisponiblesAsync(int empleadoKey);
    }
}
