using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.SantaLucia.Web.Models.Entities;

namespace HRM.SantaLucia.Web.Data.Repositories
{
    public interface IEmpleadoRepository
    {
        Task<IEnumerable<Empleado>> GetAllAsync(bool soloActivos = true);
        Task<Empleado> GetByIdAsync(int id);
        Task<Empleado> GetByEmpleadoIDAsync(string empleadoID);
        Task<Empleado> GetByCedulaAsync(string cedula);
        Task<IEnumerable<Empleado>> SearchAsync(string searchTerm, int? departamentoKey = null, bool soloActivos = true);
        Task<int> CreateAsync(Empleado empleado);
        Task<bool> UpdateAsync(Empleado empleado);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsCedulaAsync(string cedula, int? excludeId = null);
        Task<bool> ExistsEmailAsync(string email, int? excludeId = null);
    }
}