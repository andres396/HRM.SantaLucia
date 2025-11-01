namespace HRM.SantaLucia.Web.Services
{
    public interface INominaService
    {
        Task<CalcularNominaViewModel> GetNominaPeriodoAsync(int año, int mes);
        Task<bool> CalcularNominaAsync(int año, int mes, string usuarioCreacion);
        Task<IEnumerable<NominaViewModel>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12);
        Task<decimal> GetTotalNominaAsync(int año, int mes);
    }
}