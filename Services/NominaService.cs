using AutoMapper;
using HRM.SantaLucia.Web.Data.Repositories;
using HRM.SantaLucia.Web.Models.ViewModels;
using HRM.SantaLucia.Web.Models.Entities;
using OfficeOpenXml;
using System.Text;

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
            var nominas = await _repository.GetByPeriodoAsync(ano, mes, quincena, sede);
            var nombreMes = new DateTime(ano, mes, 1).ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
            var quincenaTexto = quincena == 1 ? "Primera" : "Segunda";
            var sedeTexto = sede ?? "Todas";

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Nómina");

            // Encabezado
            worksheet.Cells[1, 1].Value = $"Nómina {quincenaTexto} Quincena - {nombreMes} {ano}";
            worksheet.Cells[1, 1, 1, 22].Merge = true;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.Font.Size = 14;

            worksheet.Cells[2, 1].Value = $"Sede: {sedeTexto}";
            worksheet.Cells[2, 1, 2, 22].Merge = true;

            // Encabezados de columnas (todos los campos de nómina)
            var headers = new[]
            {
                "Empleado", "Sede", "Salario Base", "QTY Horas Regulares", "Pago Horas Regulares", "QTY Horas Extras", "Pago Horas Extras",
                "QTY Días Feriados", "Feriados (monto)", "Bonificaciones", "Comisiones", "Misceláneo", "Aguinaldo", "Extras Quincenales",
                "Total Extras", "Seguro Social", "Renta", "Otras Deducciones", "Deducciones Quincenales", "Total Deducciones", "Salario Neto"
            };
            for (int c = 0; c < headers.Length; c++)
            {
                worksheet.Cells[4, c + 1].Value = headers[c];
            }
            worksheet.Cells[4, 1, 4, headers.Length].Style.Font.Bold = true;
            worksheet.Cells[4, 1, 4, headers.Length].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[4, 1, 4, headers.Length].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

            // Datos
            int row = 5;
            foreach (var nomina in nominas.OrderBy(n => n.Empleado.NombreCompleto))
            {
                worksheet.Cells[row, 1].Value = nomina.Empleado?.NombreCompleto ?? "";
                worksheet.Cells[row, 2].Value = nomina.Sede ?? "";
                worksheet.Cells[row, 3].Value = nomina.SalarioBase;
                worksheet.Cells[row, 4].Value = nomina.QTYHorasRegulares;
                worksheet.Cells[row, 5].Value = nomina.PagoHorasRegulares;
                worksheet.Cells[row, 6].Value = nomina.QTYHorasExtras;
                worksheet.Cells[row, 7].Value = nomina.PagoHorasExtra;
                worksheet.Cells[row, 8].Value = nomina.QTYDiasFeriados;
                worksheet.Cells[row, 9].Value = nomina.Feriados;
                worksheet.Cells[row, 10].Value = nomina.Bonificaciones;
                worksheet.Cells[row, 11].Value = nomina.Comisiones;
                worksheet.Cells[row, 12].Value = nomina.Miscelaneo;
                worksheet.Cells[row, 13].Value = nomina.Aguinaldo;
                worksheet.Cells[row, 14].Value = nomina.ExtrasQuincenales;
                worksheet.Cells[row, 15].Value = nomina.TotalExtras;
                worksheet.Cells[row, 16].Value = nomina.SeguroSocial;
                worksheet.Cells[row, 17].Value = nomina.Renta;
                worksheet.Cells[row, 18].Value = nomina.OtrasDeducciones;
                worksheet.Cells[row, 19].Value = nomina.DeduccionesQuincenales;
                worksheet.Cells[row, 20].Value = nomina.TotalDeducciones;
                worksheet.Cells[row, 21].Value = nomina.SalarioNeto;
                for (int col = 3; col <= 21; col++)
                {
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,##0.00";
                }
                row++;
            }

            // Total
            worksheet.Cells[row, 1].Value = "TOTAL";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 15].Value = nominas.Sum(n => n.TotalExtras);
            worksheet.Cells[row, 20].Value = nominas.Sum(n => n.TotalDeducciones);
            worksheet.Cells[row, 21].Value = nominas.Sum(n => n.SalarioNeto);
            worksheet.Cells[row, 15].Style.Numberformat.Format = "#,##0.00";
            worksheet.Cells[row, 20].Style.Numberformat.Format = "#,##0.00";
            worksheet.Cells[row, 21].Style.Numberformat.Format = "#,##0.00";
            worksheet.Cells[row, 15, row, 21].Style.Font.Bold = true;

            // Ajustar ancho de columnas
            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
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
    }
}
