namespace HRM.SantaLucia.Web.Data.Repositories
{
    public class VacacionesRepository : IVacacionesRepository
    {
        private readonly ApplicationDbContext _context;

        public VacacionesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vacaciones>> GetPendientesAsync()
        {
            return await _context.Vacaciones
                .Include(v => v.Empleado)
                    .ThenInclude(e => e.Departamento)
                .Where(v => v.Estado == "Pendiente")
                .OrderBy(v => v.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vacaciones>> GetByEmpleadoAsync(int empleadoKey)
        {
            return await _context.Vacaciones
                .Include(v => v.Aprobador)
                .Where(v => v.EmpleadoKey == empleadoKey)
                .OrderByDescending(v => v.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<int> SolicitarAsync(Vacaciones vacacion)
        {
            _context.Vacaciones.Add(vacacion);
            await _context.SaveChangesAsync();
            return vacacion.VacacionKey;
        }

        public async Task<bool> AprobarRechazarAsync(int vacacionKey, int aprobadorKey, string estado, string observaciones)
        {
            var vacacion = await _context.Vacaciones.FindAsync(vacacionKey);
            if (vacacion == null)
                return false;

            vacacion.Estado = estado;
            vacacion.FechaAprobacion = DateTime.Now;
            vacacion.AprobadorKey = aprobadorKey;
            vacacion.Observaciones = observaciones;

            if (estado == "Aprobado")
            {
                vacacion.DiasTomados = vacacion.DiasSolicitados;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetDiasDisponiblesAsync(int empleadoKey)
        {
            var empleado = await _context.Empleados.FindAsync(empleadoKey);
            if (empleado == null)
                return 0;

            var mesesTrabajados = (DateTime.Now.Year - empleado.FechaIngreso.Year) * 12 +
                                  DateTime.Now.Month - empleado.FechaIngreso.Month;

            var diasAcumulados = (int)Math.Floor(mesesTrabajados * 1.25);

            var diasTomados = await _context.Vacaciones
                .Where(v => v.EmpleadoKey == empleadoKey && v.Estado == "Aprobado")
                .SumAsync(v => v.DiasTomados);

            return diasAcumulados - diasTomados;
        }
    }
}