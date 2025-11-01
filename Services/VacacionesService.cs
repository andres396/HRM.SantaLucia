namespace HRM.SantaLucia.Web.Services
{
    public class VacacionesService : IVacacionesService
    {
        private readonly IVacacionesRepository _repository;
        private readonly IMapper _mapper;

        public VacacionesService(IVacacionesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<VacacionesPendientesViewModel> GetPendientesAsync()
        {
            var vacaciones = await _repository.GetPendientesAsync();
            var vacacionesViewModel = _mapper.Map<List<VacacionesViewModel>>(vacaciones);

            return new VacacionesPendientesViewModel
            {
                Solicitudes = vacacionesViewModel,
                TotalPendientes = vacacionesViewModel.Count
            };
        }

        public async Task<IEnumerable<VacacionesViewModel>> GetByEmpleadoAsync(int empleadoKey)
        {
            var vacaciones = await _repository.GetByEmpleadoAsync(empleadoKey);
            return _mapper.Map<IEnumerable<VacacionesViewModel>>(vacaciones);
        }

        public async Task<int> SolicitarAsync(VacacionesViewModel model)
        {
            var diasDisponibles = await _repository.GetDiasDisponiblesAsync(model.EmpleadoKey);

            if (model.DiasSolicitados > diasDisponibles)
            {
                throw new InvalidOperationException("No tiene suficientes días de vacaciones disponibles");
            }

            var vacacion = _mapper.Map<Vacaciones>(model);
            vacacion.FechaInicioKey = int.Parse(model.FechaInicio.ToString("yyyyMMdd"));
            vacacion.FechaFinKey = int.Parse(model.FechaFin.ToString("yyyyMMdd"));
            vacacion.DiasDisponibles = diasDisponibles;
            vacacion.DiasTomados = 0;
            vacacion.DiasRestantes = diasDisponibles - model.DiasSolicitados;
            vacacion.Estado = "Pendiente";
            vacacion.FechaSolicitud = DateTime.Now;

            return await _repository.SolicitarAsync(vacacion);
        }

        public async Task<bool> AprobarAsync(int vacacionKey, int aprobadorKey, string observaciones)
        {
            return await _repository.AprobarRechazarAsync(vacacionKey, aprobadorKey, "Aprobado", observaciones);
        }

        public async Task<bool> RechazarAsync(int vacacionKey, int aprobadorKey, string observaciones)
        {
            return await _repository.AprobarRechazarAsync(vacacionKey, aprobadorKey, "Rechazado", observaciones);
        }

        public async Task<int> GetDiasDisponiblesAsync(int empleadoKey)
        {
            return await _repository.GetDiasDisponiblesAsync(empleadoKey);
        }
    }
}