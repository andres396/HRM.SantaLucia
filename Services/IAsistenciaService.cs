using HRM.SantaLucia.Web.Models.ViewModels;

namespace HRM.SantaLucia.Web.Services
{
    public interface IAsistenciaService
    {
        Task<AsistenciaDiariaViewModel> GetAsistenciaDiariaAsync(DateTime fecha);
        Task<bool> RegistrarAsistenciaAsync(AsistenciaViewModel model);
        Task<IEnumerable<AsistenciaViewModel>> GetHistorialEmpleadoAsync(int empleadoKey, DateTime? desde = null, DateTime? hasta = null);
        Task<IEnumerable<AsistenciaViewModel>> GetResumenMensualAsync(int ano, int mes);
    }
}
