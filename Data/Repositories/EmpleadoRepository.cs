using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.SantaLucia.Web.Models.Entities;

namespace HRM.SantaLucia.Web.Data.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleado>> GetAllAsync(bool soloActivos = true)
        {
            var query = _context.Empleados
                .Include(e => e.Puesto)
                .Include(e => e.Departamento)
                .Include(e => e.Banco)
                .AsQueryable();

            if (soloActivos)
            {
                query = query.Where(e => e.Activo);
            }

            return await query.OrderBy(e => e.NombreCompleto).ToListAsync();
        }

        public async Task<Empleado> GetByIdAsync(int id)
        {
            return await _context.Empleados
                .Include(e => e.Puesto)
                .Include(e => e.Departamento)
                .Include(e => e.Banco)
                .FirstOrDefaultAsync(e => e.EmpleadoKey == id);
        }

        public async Task<Empleado> GetByEmpleadoIDAsync(string empleadoID)
        {
            return await _context.Empleados
                .Include(e => e.Puesto)
                .Include(e => e.Departamento)
                .FirstOrDefaultAsync(e => e.EmpleadoID == empleadoID);
        }

        public async Task<Empleado> GetByCedulaAsync(string cedula)
        {
            return await _context.Empleados
                .FirstOrDefaultAsync(e => e.Cedula == cedula);
        }

        public async Task<IEnumerable<Empleado>> SearchAsync(string searchTerm, int? departamentoKey = null, bool soloActivos = true)
        {
            var query = _context.Empleados
                .Include(e => e.Puesto)
                .Include(e => e.Departamento)
                .AsQueryable();

            if (soloActivos)
            {
                query = query.Where(e => e.Activo);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e =>
                    e.NombreCompleto.Contains(searchTerm) ||
                    e.Cedula.Contains(searchTerm) ||
                    e.EmpleadoID.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm));
            }

            if (departamentoKey.HasValue)
            {
                query = query.Where(e => e.DepartamentoKey == departamentoKey.Value);
            }

            return await query.OrderBy(e => e.NombreCompleto).ToListAsync();
        }

        public async Task<int> CreateAsync(Empleado empleado)
        {
            try
            {
                // Asegurar que las propiedades de navegación no se incluyan
                empleado.Puesto = null;
                empleado.Departamento = null;
                empleado.Banco = null;
                
                // Asegurar que EmpleadoKey sea 0 para nueva entidad
                empleado.EmpleadoKey = 0;
                
                _context.Empleados.Add(empleado);
                
                // Marcar las columnas calculadas como no modificadas para que EF no intente guardarlas
                var entry = _context.Entry(empleado);
                entry.Property(e => e.NombreCompleto).IsModified = false;
                entry.Property(e => e.Edad).IsModified = false;
                
                await _context.SaveChangesAsync();
                return empleado.EmpleadoKey;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                // Capturar errores específicos de base de datos
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                throw new Exception($"Error de base de datos al guardar el empleado: {innerMessage}", dbEx);
            }
            catch (Exception ex)
            {
                // Log del error completo para debugging
                throw new Exception($"Error al guardar el empleado: {ex.Message}. Inner Exception: {ex.InnerException?.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(Empleado empleado)
        {
            empleado.FechaModificacion = DateTime.Now;
            _context.Entry(empleado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(empleado.EmpleadoKey))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return false;
            }

            // Eliminaci�n l�gica
            empleado.Activo = false;
            empleado.FechaSalida = DateTime.Now;
            empleado.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Empleados.AnyAsync(e => e.EmpleadoKey == id);
        }

        public async Task<bool> ExistsCedulaAsync(string cedula, int? excludeId = null)
        {
            var query = _context.Empleados.Where(e => e.Cedula == cedula);

            if (excludeId.HasValue)
            {
                query = query.Where(e => e.EmpleadoKey != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsEmailAsync(string email, int? excludeId = null)
        {
            var query = _context.Empleados.Where(e => e.Email == email);

            if (excludeId.HasValue)
            {
                query = query.Where(e => e.EmpleadoKey != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}