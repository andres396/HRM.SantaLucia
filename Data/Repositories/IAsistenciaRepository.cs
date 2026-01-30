using HRM.SantaLucia.Web.Models.Entities;

namespace HRM.SantaLucia.Web.Data.Repositories
{
    public interface IAsistenciaRepository
    {
        Task<IEnumerable<Asistencia>> GetByFechaAsync(DateTime fecha);
        Task<Asistencia> GetByEmpleadoYFechaAsync(int empleadoKey, DateTime fecha);
        Task<bool> RegistrarAsync(Asistencia asistencia);
        Task<IEnumerable<Asistencia>> GetByEmpleadoAsync(int empleadoKey, DateTime? desde = null, DateTime? hasta = null);
        Task<IEnumerable<Asistencia>> GetByPeriodoAsync(DateTime desde, DateTime hasta);
        Task<Dictionary<string, int>> GetResumenMensualAsync(int ano, int mes);
    }
}
