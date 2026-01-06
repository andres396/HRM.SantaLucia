using AutoMapper;
using HRM.SantaLucia.Web.Data.Repositories;
using HRM.SantaLucia.Web.Models.ViewModels;

namespace HRM.SantaLucia.Web.Services
{
    public class NominaService : INominaService
    {
        private readonly INominaRepository _repository;
        private readonly IMapper _mapper;

        public NominaService(INominaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CalcularNominaViewModel> GetNominaPeriodoAsync(int ano, int mes)
        {
            var nominas = await _repository.GetByPeriodoAsync(ano, mes);
            var nominasViewModel = _mapper.Map<List<NominaViewModel>>(nominas);

            // Agregar nombre del mes
            var nombreMes = new DateTime(ano, mes, 1).ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));

            foreach (var nomina in nominasViewModel)
            {
                nomina.Ano = ano;
                nomina.Mes = mes;
                nomina.NombreMes = nombreMes;
            }

            return new CalcularNominaViewModel
            {
                Ano = ano,
                Mes = mes,
                Nominas = nominasViewModel,
                TotalAPagar = nominasViewModel.Sum(n => n.SalarioNeto),
                TotalEmpleados = nominasViewModel.Count
            };
        }

        public async Task<bool> CalcularNominaAsync(int ano, int mes, string usuarioCreacion)
        {
            return await _repository.CalcularNominaAsync(ano, mes, usuarioCreacion);
        }

        public async Task<IEnumerable<NominaViewModel>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12)
        {
            var nominas = await _repository.GetHistorialEmpleadoAsync(empleadoKey, top);
            var nominasViewModel = _mapper.Map<List<NominaViewModel>>(nominas);

            // Extraer año y mes del PeriodoKey (formato yyyyMM) y establecer NombreMes
            foreach (var (nomina, nominaViewModel) in nominas.Zip(nominasViewModel))
            {
                var periodoKey = nomina.PeriodoKey;
                var ano = periodoKey / 100;
                var mes = periodoKey % 100;
                
                nominaViewModel.Ano = ano;
                nominaViewModel.Mes = mes;
                nominaViewModel.NombreMes = new DateTime(ano, mes, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-ES"));
            }

            return nominasViewModel;
        }

        public async Task<decimal> GetTotalNominaAsync(int ano, int mes)
        {
            return await _repository.GetTotalNominaAsync(ano, mes);
        }
    }
}
