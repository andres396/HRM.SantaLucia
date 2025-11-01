using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.SantaLucia.Web.Models.ViewModels;

namespace HRM.SantaLucia.Web.Services
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<EmpleadoViewModel>> GetAllAsync(bool soloActivos = true);
        Task<EmpleadoViewModel> GetByIdAsync(int id);
        Task<int> CreateAsync(EmpleadoViewModel model, string usuarioCreacion);
        Task<bool> UpdateAsync(EmpleadoViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<EmpleadoViewModel>> SearchAsync(string searchTerm, int? departamentoKey = null, bool soloActivos = true);
        Task<bool> ValidarCedulaUnicaAsync(string cedula, int? excludeId = null);
        Task<bool> ValidarEmailUnicoAsync(string email, int? excludeId = null);
    }
}