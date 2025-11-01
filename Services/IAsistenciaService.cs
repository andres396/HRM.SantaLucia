namespace HRM.SantaLucia.Web.Services
{
    public interface IAsistenciaService
    {
        Task<AsistenciaDiariaViewModel> GetAsistenciaDiariaAsync(DateTime fecha);
        Task<bool> RegistrarAsistenciaAsync(AsistenciaViewModel model);
        Task<IEnumerable<AsistenciaViewModel>> GetHistorialEmpleadoAsync(int empleadoKey, DateTime? desde = null, DateTime? hasta = null);
        Task<Dictionary<string, int>> GetResumenMensualAsync(int año, int mes);
    }
}