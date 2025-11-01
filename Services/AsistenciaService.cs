namespace HRM.SantaLucia.Web.Services
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IAsistenciaRepository _repository;
        private readonly IMapper _mapper;

        public AsistenciaService(IAsistenciaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AsistenciaDiariaViewModel> GetAsistenciaDiariaAsync(DateTime fecha)
        {
            var asistencias = await _repository.GetByFechaAsync(fecha);
            var asistenciasViewModel = _mapper.Map<List<AsistenciaViewModel>>(asistencias);

            return new AsistenciaDiariaViewModel
            {
                Fecha = fecha,
                Asistencias = asistenciasViewModel,
                TotalPresentes = asistenciasViewModel.Count(a => a.Estado == "Presente"),
                TotalAusentes = asistenciasViewModel.Count(a => a.Estado == "Ausente"),
                TotalTarde = asistenciasViewModel.Count(a => a.Estado == "Tarde")
            };
        }

        public async Task<bool> RegistrarAsistenciaAsync(AsistenciaViewModel model)
        {
            var asistencia = _mapper.Map<Asistencia>(model);
            asistencia.FechaKey = int.Parse(model.Fecha.ToString("yyyyMMdd"));
            asistencia.FechaRegistro = DateTime.Now;

            // Calcular horas trabajadas y minutos tarde
            if (model.HoraEntrada.HasValue && model.HoraSalida.HasValue)
            {
                var horasTrabajadas = (model.HoraSalida.Value - model.HoraEntrada.Value).TotalHours;
                asistencia.HorasTrabajadas = (decimal)horasTrabajadas;

                // Calcular minutos de retraso (asumiendo entrada a las 7:00 AM)
                var horaEntradaEsperada = new TimeSpan(7, 0, 0);
                if (model.HoraEntrada.Value > horaEntradaEsperada)
                {
                    asistencia.MinutosTarde = (int)(model.HoraEntrada.Value - horaEntradaEsperada).TotalMinutes;
                }
            }

            return await _repository.RegistrarAsync(asistencia);
        }

        public async Task<IEnumerable<AsistenciaViewModel>> GetHistorialEmpleadoAsync(int empleadoKey, DateTime? desde = null, DateTime? hasta = null)
        {
            var asistencias = await _repository.GetByEmpleadoAsync(empleadoKey, desde, hasta);
            return _mapper.Map<IEnumerable<AsistenciaViewModel>>(asistencias);
        }

        public async Task<Dictionary<string, int>> GetResumenMensualAsync(int año, int mes)
        {
            return await _repository.GetResumenMensualAsync(año, mes);
        }
    }
}