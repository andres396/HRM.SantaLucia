namespace HRM.SantaLucia.Web.Data.Repositories
{
    public interface IVacacionesRepository
    {
        Task<IEnumerable<Vacaciones>> GetPendientesAsync();
        Task<IEnumerable<Vacaciones>> GetByEmpleadoAsync(int empleadoKey);
        Task<int> SolicitarAsync(Vacaciones vacacion);
        Task<bool> AprobarRechazarAsync(int vacacionKey, int aprobadorKey, string estado, string observaciones);
        Task<int> GetDiasDisponiblesAsync(int empleadoKey);
    }
}