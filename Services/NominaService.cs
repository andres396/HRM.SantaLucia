using AutoMapper;
using HRM.SantaLucia.Web.Data.Repositories;
using HRM.SantaLucia.Web.Helpers;
using HRM.SantaLucia.Web.Models.ViewModels;
using HRM.SantaLucia.Web.Models.Entities;
using OfficeOpenXml;
using System.Globalization;

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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<CalcularNominaViewModel> GetNominaPeriodoAsync(int ano, int mes, int quincena, string? sede)
        {
            var nominas = await _repository.GetByPeriodoAsync(ano, mes, quincena, sede);
            var nominasViewModel = _mapper.Map<List<NominaViewModel>>(nominas);

            var nombreMes = new DateTime(ano, mes, 1).ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
            var quincenaTexto = quincena == 1 ? "Primera" : "Segunda";

            foreach (var nomina in nominasViewModel)
            {
                nomina.Ano = ano;
                nomina.Mes = mes;
                nomina.Dia = quincena == 1 ? 15 : DateTime.DaysInMonth(ano, mes);
                nomina.NombreMes = nombreMes;
                nomina.Quincena = quincenaTexto;
            }

            return new CalcularNominaViewModel
            {
                Ano = ano,
                Mes = mes,
                Quincena = quincena,
                Sede = sede ?? "",
                Nominas = nominasViewModel,
                TotalAPagar = nominasViewModel.Sum(n => n.SalarioNeto),
                TotalEmpleados = nominasViewModel.Count
            };
        }

        public async Task<bool> CalcularNominaAsync(int ano, int mes, int quincena, string sede, string usuarioCreacion)
        {
            return await _repository.CalcularNominaAsync(ano, mes, quincena, sede, usuarioCreacion);
        }

        public async Task<NominaViewModel?> GetByIdAsync(int nominaKey)
        {
            var nomina = await _repository.GetByIdAsync(nominaKey);
            if (nomina == null)
                return null;

            var viewModel = _mapper.Map<NominaViewModel>(nomina);
            
            // Extraer información del PeriodoKey (yyyyMMdd)
            var periodoKeyStr = nomina.PeriodoKey.ToString();
            if (periodoKeyStr.Length == 8)
            {
                viewModel.Ano = int.Parse(periodoKeyStr.Substring(0, 4));
                viewModel.Mes = int.Parse(periodoKeyStr.Substring(4, 2));
                viewModel.Dia = int.Parse(periodoKeyStr.Substring(6, 2));
                viewModel.Quincena = viewModel.Dia == 15 ? "Primera" : "Segunda";
            }

            return viewModel;
        }

        public async Task<bool> UpdateNominaAsync(EditarNominaViewModel model)
        {
            var nomina = await _repository.GetByIdAsync(model.NominaKey);
            if (nomina == null)
                return false;

            nomina.QTYHorasExtras = model.QTYHorasExtras;
            nomina.QTYHorasRegulares = model.QTYHorasRegulares;
            nomina.QTYDiasFeriados = model.QTYDiasFeriados;
            nomina.Miscelaneo = model.Miscelaneo;
            nomina.ExtrasQuincenales = model.ExtrasQuincenales;
            nomina.DeduccionesQuincenales = model.DeduccionesQuincenales;
            // Aguinaldo y Feriados (monto) se calculan automáticamente en el repositorio

            return await _repository.UpdateAsync(nomina);
        }

        public async Task<byte[]> ExportarNominaExcelAsync(int ano, int mes, int quincena, string? sede)
        {
            const int colCount = 10;
            var nominas = await _repository.GetByPeriodoAsync(ano, mes, quincena, sede);
            var nombreMes = new DateTime(ano, mes, 1).ToString("MMMM", new CultureInfo("es-ES"));
            var quincenaTexto = quincena == 1 ? "Primera" : "Segunda";
            var sedeTexto = sede ?? "Todas";
            var rangoQuincena = FormatearRangoQuincena(ano, mes, quincena);

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Nómina");

            worksheet.Cells[1, 1].Value = $"Nómina {quincenaTexto} Quincena - {nombreMes} {ano}";
            worksheet.Cells[1, 1, 1, colCount].Merge = true;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.Font.Size = 14;

            worksheet.Cells[2, 1].Value = $"Sede: {sedeTexto}";
            worksheet.Cells[2, 1, 2, colCount].Merge = true;

            var headers = new[]
            {
                "Quincena (fechas)",
                "Empleado",
                "Código Empleado",
                "Banco",
                "Cuenta Bancaria",
                "Salario Bruto Mensual",
                "Salario Bruto Quincenal",
                "Créditos",
                "Débitos",
                "Total a pagar"
            };

            for (int c = 0; c < headers.Length; c++)
            {
                worksheet.Cells[4, c + 1].Value = headers[c];
            }

            worksheet.Cells[4, 1, 4, colCount].Style.Font.Bold = true;
            worksheet.Cells[4, 1, 4, colCount].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[4, 1, 4, colCount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

            var lista = nominas.OrderBy(n => n.Empleado?.NombreCompleto ?? "").ToList();
            int row = 5;
            foreach (var nomina in lista)
            {
                var emp = nomina.Empleado;
                var salarioQuincenal = NominaPlanillaCalculo.SalarioQuincenal(nomina.SalarioBase);
                var credito = NominaPlanillaCalculo.Credito(nomina);
                var debito = NominaPlanillaCalculo.Debito(nomina);

                worksheet.Cells[row, 1].Value = rangoQuincena;
                worksheet.Cells[row, 2].Value = emp?.NombreCompleto ?? "";
                worksheet.Cells[row, 3].Value = emp?.EmpleadoID ?? "";
                worksheet.Cells[row, 4].Value = emp?.Banco?.NombreBanco ?? "";
                worksheet.Cells[row, 5].Value = emp?.CuentaBancaria ?? "";
                worksheet.Cells[row, 6].Value = nomina.SalarioBase;
                worksheet.Cells[row, 7].Value = salarioQuincenal;
                worksheet.Cells[row, 8].Value = credito;
                worksheet.Cells[row, 9].Value = debito;
                worksheet.Cells[row, 10].Value = nomina.SalarioNeto;

                for (int col = 6; col <= colCount; col++)
                {
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,##0.00";
                }

                row++;
            }

            worksheet.Cells[row, 1].Value = "TOTAL";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 6].Value = lista.Sum(n => n.SalarioBase);
            worksheet.Cells[row, 7].Value = lista.Sum(n => NominaPlanillaCalculo.SalarioQuincenal(n.SalarioBase));
            worksheet.Cells[row, 8].Value = lista.Sum(n => NominaPlanillaCalculo.Credito(n));
            worksheet.Cells[row, 9].Value = lista.Sum(n => NominaPlanillaCalculo.Debito(n));
            worksheet.Cells[row, 10].Value = lista.Sum(n => n.SalarioNeto);
            foreach (var c in new[] { 6, 7, 8, 9, 10 })
            {
                worksheet.Cells[row, c].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[row, c].Style.Font.Bold = true;
            }

            worksheet.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }

        private static string FormatearRangoQuincena(int ano, int mes, int quincena)
        {
            if (quincena == 1)
            {
                var ini = new DateTime(ano, mes, 1);
                var fin = new DateTime(ano, mes, 15);
                return $"{ini:dd/MM/yyyy} - {fin:dd/MM/yyyy}";
            }

            var ini2 = new DateTime(ano, mes, 16);
            var fin2 = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
            return $"{ini2:dd/MM/yyyy} - {fin2:dd/MM/yyyy}";
        }

        public async Task<IEnumerable<NominaViewModel>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12)
        {
            var nominas = await _repository.GetHistorialEmpleadoAsync(empleadoKey, top);
            var nominasViewModel = _mapper.Map<List<NominaViewModel>>(nominas);

            foreach (var (nomina, nominaViewModel) in nominas.Zip(nominasViewModel))
            {
                var periodoKeyStr = nomina.PeriodoKey.ToString();
                if (periodoKeyStr.Length == 8)
                {
                    nominaViewModel.Ano = int.Parse(periodoKeyStr.Substring(0, 4));
                    nominaViewModel.Mes = int.Parse(periodoKeyStr.Substring(4, 2));
                    nominaViewModel.Dia = int.Parse(periodoKeyStr.Substring(6, 2));
                    nominaViewModel.Quincena = nominaViewModel.Dia == 15 ? "Primera" : "Segunda";
                    var nombreMes = new DateTime(nominaViewModel.Ano, nominaViewModel.Mes, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-ES"));
                    nominaViewModel.NombreMes = $"{nominaViewModel.Quincena} Quincena - {nombreMes}";
                }
            }

            return nominasViewModel;
        }

        public async Task<decimal> GetTotalNominaAsync(int ano, int mes, int quincena, string? sede)
        {
            return await _repository.GetTotalNominaAsync(ano, mes, quincena, sede);
        }

        public async Task<CalcularAguinaldoViewModel> CalcularAguinaldoAsync(int ano, string? sede)
        {
            var fechaInicio = new DateTime(ano - 1, 12, 1);
            var fechaFin = new DateTime(ano, 11, DateTime.DaysInMonth(ano, 11));
            var resultados = await _repository.CalcularAguinaldoAsync(ano, sede);

            return new CalcularAguinaldoViewModel
            {
                Ano = ano,
                Sede = sede,
                FechaInicioPeriodo = fechaInicio,
                FechaFinPeriodo = fechaFin,
                Resultados = resultados,
                TotalAguinaldo = resultados.Sum(r => r.AguinaldoCalculado)
            };
        }
    }
}
