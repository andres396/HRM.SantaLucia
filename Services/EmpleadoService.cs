using AutoMapper;
using HRM.SantaLucia.Web.Data.Repositories;
using HRM.SantaLucia.Web.Models.Entities;
using HRM.SantaLucia.Web.Models.ViewModels;
using HRM.SantaLucia.Web.Data;

namespace HRM.SantaLucia.Web.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _repository;
        private readonly IMapper _mapper;

        public EmpleadoService(IEmpleadoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmpleadoViewModel>> GetAllAsync(bool soloActivos = true)
        {
            var empleados = await _repository.GetAllAsync(soloActivos);
            return _mapper.Map<IEnumerable<EmpleadoViewModel>>(empleados);
        }

        public async Task<EmpleadoViewModel> GetByIdAsync(int id)
        {
            var empleado = await _repository.GetByIdAsync(id);
            return _mapper.Map<EmpleadoViewModel>(empleado);
        }

        public async Task<int> CreateAsync(EmpleadoViewModel model, string usuarioCreacion)
        {
            var empleado = _mapper.Map<Empleado>(model);
            
            // Asegurar que las propiedades de navegación sean null (solo usar foreign keys)
            empleado.Puesto = null;
            empleado.Departamento = null;
            empleado.Banco = null;
            
            // Asegurar que las propiedades calculadas sean null (se calcularán en la BD)
            empleado.NombreCompleto = null;
            empleado.Edad = null;
            
            // Establecer valores de auditoría
            empleado.UsuarioCreacion = usuarioCreacion;
            empleado.FechaCreacion = DateTime.Now;
            empleado.Activo = true;
            empleado.EmpleadoKey = 0; // Asegurar que es una nueva entidad

            return await _repository.CreateAsync(empleado);
        }

        public async Task<bool> UpdateAsync(EmpleadoViewModel model)
        {
            var empleado = await _repository.GetByIdAsync(model.EmpleadoKey);
            if (empleado == null)
                return false;

            // Actualizar solo las propiedades editables (no las de navegación ni calculadas)
            empleado.EmpleadoID = model.EmpleadoID;
            empleado.Cedula = model.Cedula;
            empleado.Nombre = model.Nombre;
            empleado.Apellido1 = model.Apellido1;
            empleado.Apellido2 = model.Apellido2;
            empleado.FechaNacimiento = model.FechaNacimiento;
            empleado.Genero = model.Genero;
            empleado.EstadoCivil = model.EstadoCivil;
            empleado.Nacionalidad = model.Nacionalidad;
            empleado.Email = model.Email;
            empleado.EmailPersonal = model.EmailPersonal;
            empleado.Telefono = model.Telefono;
            empleado.TelefonoEmergencia = model.TelefonoEmergencia;
            empleado.ContactoEmergencia = model.ContactoEmergencia;
            empleado.Provincia = model.Provincia;
            empleado.Canton = model.Canton;
            empleado.Distrito = model.Distrito;
            empleado.DireccionExacta = model.DireccionExacta;
            empleado.FechaIngreso = model.FechaIngreso;
            empleado.FechaSalida = model.FechaSalida;
            empleado.TipoContrato = model.TipoContrato;
            empleado.PuestoKey = model.PuestoKey;
            empleado.DepartamentoKey = model.DepartamentoKey;
            empleado.BancoKey = model.BancoKey;
            empleado.CuentaBancaria = model.CuentaBancaria;
            empleado.Activo = model.Activo;
            empleado.Sede = model.Sede;
            empleado.SalarioBase = model.SalarioBase;
            empleado.SalarioNeto = model.SalarioNeto;
            empleado.Rebajos = model.Rebajos;
            empleado.CCSS = model.CCSS;
            empleado.JUPEMA = model.JUPEMA;
            empleado.Magisterio = model.Magisterio;
            empleado.PorcentajeBP = model.PorcentajeBP;
            empleado.Bonos = model.Bonos;
            
            empleado.FechaModificacion = DateTime.Now;
            empleado.UsuarioModificacion = "SYSTEM"; // TODO: Pasar usuario desde el controlador

            return await _repository.UpdateAsync(empleado);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<EmpleadoViewModel>> SearchAsync(string searchTerm, int? departamentoKey = null, bool soloActivos = true)
        {
            var empleados = await _repository.SearchAsync(searchTerm, departamentoKey, soloActivos);
            return _mapper.Map<IEnumerable<EmpleadoViewModel>>(empleados);
        }

        public async Task<bool> ValidarCedulaUnicaAsync(string cedula, int? excludeId = null)
        {
            return !await _repository.ExistsCedulaAsync(cedula, excludeId);
        }

        public async Task<bool> ValidarEmailUnicoAsync(string email, int? excludeId = null)
        {
            return !await _repository.ExistsEmailAsync(email, excludeId);
        }
    }
}