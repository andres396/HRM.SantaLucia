using AutoMapper;
using HRM.SantaLucia.Web.Data.Repositories;
using HRM.SantaLucia.Web.Models.Entities;
using HRM.SantaLucia.Web.Models.ViewModels;

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
            asistencia.FechaCreacion = DateTime.Now;
            asistencia.UsuarioCreacion = "SYSTEM"; // TODO: Obtener del usuario actual autenticado

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

        public async Task<IEnumerable<AsistenciaViewModel>> GetResumenMensualAsync(int ano, int mes)
        {
            var desde = new DateTime(ano, mes, 1);
            var hasta = desde.AddMonths(1).AddDays(-1);
            var asistencias = await _repository.GetByPeriodoAsync(desde, hasta);
            return _mapper.Map<IEnumerable<AsistenciaViewModel>>(asistencias);
        }
    }
}
