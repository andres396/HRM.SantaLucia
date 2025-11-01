namespace HRM.SantaLucia.Web.Data.Repositories
{
    public interface INominaRepository
    {
        Task<IEnumerable<Nomina>> GetByPeriodoAsync(int año, int mes);
        Task<Nomina> GetByEmpleadoYPeriodoAsync(int empleadoKey, int año, int mes);
        Task<bool> CalcularNominaAsync(int año, int mes, string usuarioCreacion);
        Task<decimal> GetTotalNominaAsync(int año, int mes);
        Task<IEnumerable<Nomina>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12);
    }
}