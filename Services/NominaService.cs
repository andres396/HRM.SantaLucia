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

        public async Task<CalcularNominaViewModel> GetNominaPeriodoAsync(int año, int mes)
        {
            var nominas = await _repository.GetByPeriodoAsync(año, mes);
            var nominasViewModel = _mapper.Map<List<NominaViewModel>>(nominas);

            // Agregar nombre del mes
            var nombreMes = new DateTime(año, mes, 1).ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));

            foreach (var nomina in nominasViewModel)
            {
                nomina.Año = año;
                nomina.Mes = mes;
                nomina.NombreMes = nombreMes;
            }

            return new CalcularNominaViewModel
            {
                Año = año,
                Mes = mes,
                Nominas = nominasViewModel,
                TotalAPagar = nominasViewModel.Sum(n => n.SalarioNeto),
                TotalEmpleados = nominasViewModel.Count
            };
        }

        public async Task<bool> CalcularNominaAsync(int año, int mes, string usuarioCreacion)
        {
            return await _repository.CalcularNominaAsync(año, mes, usuarioCreacion);
        }

        public async Task<IEnumerable<NominaViewModel>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12)
        {
            var nominas = await _repository.GetHistorialEmpleadoAsync(empleadoKey, top);
            return _mapper.Map<IEnumerable<NominaViewModel>>(nominas);
        }

        public async Task<decimal> GetTotalNominaAsync(int año, int mes)
        {
            return await _repository.GetTotalNominaAsync(año, mes);
        }
    }
}