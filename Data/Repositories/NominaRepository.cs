using Microsoft.EntityFrameworkCore;
using HRM.SantaLucia.Web.Data;
using HRM.SantaLucia.Web.Models.Entities;
using HRM.SantaLucia.Web.Models.ViewModels;
using HRM.SantaLucia.Web.Helpers;

namespace HRM.SantaLucia.Web.Data.Repositories
{
    public class NominaRepository : INominaRepository
    {
        private readonly ApplicationDbContext _context;

        public NominaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Nomina>> GetByPeriodoAsync(int ano, int mes, int quincena, string? sede)
        {
            // Calcular el día de la quincena
            int diaQuincena = quincena == 1 ? 15 : DateTime.DaysInMonth(ano, mes);
            var periodoKey = ano * 10000 + mes * 100 + diaQuincena;

            var query = _context.Nominas
                .Include(n => n.Empleado)
                    .ThenInclude(e => e.Banco)
                .Include(n => n.Puesto)
                .Include(n => n.Departamento)
                .Where(n => n.PeriodoKey == periodoKey);

            if (!string.IsNullOrEmpty(sede))
            {
                query = query.Where(n => n.Sede == sede);
            }

            return await query
                .OrderBy(n => n.Empleado.NombreCompleto)
                .ToListAsync();
        }

        public async Task<Nomina> GetByEmpleadoYPeriodoAsync(int empleadoKey, int periodoKey)
        {
            return await _context.Nominas
                .Include(n => n.Empleado)
                .Include(n => n.Puesto)
                .Include(n => n.Departamento)
                .FirstOrDefaultAsync(n => n.EmpleadoKey == empleadoKey && n.PeriodoKey == periodoKey);
        }

        public async Task<Nomina?> GetByIdAsync(int nominaKey)
        {
            return await _context.Nominas
                .Include(n => n.Empleado)
                .Include(n => n.Puesto)
                .Include(n => n.Departamento)
                .FirstOrDefaultAsync(n => n.NominaKey == nominaKey);
        }

        public async Task<bool> CalcularNominaAsync(int ano, int mes, int quincena, string sede, string usuarioCreacion)
        {
            try
            {
                // Calcular el día de la quincena
                int diaQuincena = quincena == 1 ? 15 : DateTime.DaysInMonth(ano, mes);
                var periodoKey = ano * 10000 + mes * 100 + diaQuincena;

                // Obtener empleados activos de la sede especificada
                var empleados = await _context.Empleados
                    .Include(e => e.Puesto)
                    .Include(e => e.Departamento)
                    .Where(e => e.Activo 
                        && e.Sede == sede
                        && e.FechaIngreso <= new DateTime(ano, mes, diaQuincena)
                        && (!e.FechaSalida.HasValue || e.FechaSalida.Value >= new DateTime(ano, mes, diaQuincena)))
                    .ToListAsync();

                foreach (var empleado in empleados)
                {
                    if (!empleado.PuestoKey.HasValue || !empleado.DepartamentoKey.HasValue)
                    {
                        continue;
                    }

                    var existing = await _context.Nominas
                        .FirstOrDefaultAsync(n => n.EmpleadoKey == empleado.EmpleadoKey && n.PeriodoKey == periodoKey);

                    if (existing == null)
                    {
                        var nomina = new Nomina
                        {
                            EmpleadoKey = empleado.EmpleadoKey,
                            PuestoKey = empleado.PuestoKey.Value,
                            DepartamentoKey = empleado.DepartamentoKey.Value,
                            PeriodoKey = periodoKey,
                            Sede = sede,
                            QTYHorasRegulares = 80m,
                            QTYHorasExtras = 0m,
                            QTYDiasFeriados = 0m,
                            ExtrasQuincenales = 0m,
                            DeduccionesQuincenales = 0m,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = usuarioCreacion
                        };

                        AplicarFormulaQuincenal(nomina, empleado, sincronizarMiscelaneoConBonos: true);
                        _context.Nominas.Add(nomina);
                    }
                    else
                    {
                        AplicarFormulaQuincenal(existing, empleado, sincronizarMiscelaneoConBonos: false);
                        existing.FechaModificacion = DateTime.Now;
                        existing.UsuarioModificacion = usuarioCreacion;
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Nomina nomina)
        {
            try
            {
                var existing = await _context.Nominas.FindAsync(nomina.NominaKey);
                if (existing == null)
                    return false;

                var empleado = await _context.Empleados
                    .Include(e => e.Puesto)
                    .FirstOrDefaultAsync(e => e.EmpleadoKey == existing.EmpleadoKey);

                if (empleado == null)
                {
                    return false;
                }

                // Actualizar campos editables
                existing.QTYHorasExtras = nomina.QTYHorasExtras;
                existing.QTYHorasRegulares = nomina.QTYHorasRegulares;
                existing.QTYDiasFeriados = nomina.QTYDiasFeriados;
                existing.Miscelaneo = nomina.Miscelaneo;
                existing.ExtrasQuincenales = nomina.ExtrasQuincenales;
                existing.DeduccionesQuincenales = nomina.DeduccionesQuincenales;
                AplicarFormulaQuincenal(existing, empleado, sincronizarMiscelaneoConBonos: false);

                existing.FechaModificacion = DateTime.Now;
                existing.UsuarioModificacion = "SYSTEM";

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void AplicarFormulaQuincenal(Nomina nomina, Empleado empleado, bool sincronizarMiscelaneoConBonos)
        {
            var salarioBase = empleado.SalarioBase ?? empleado.Puesto?.SalarioMinimo ?? 0m;
            var horaRegular = NominaPlanillaCalculo.ValorHoraRegular(salarioBase);

            nomina.SalarioBase = salarioBase;
            nomina.Sede = empleado.Sede;
            nomina.PuestoKey = empleado.PuestoKey ?? nomina.PuestoKey;
            nomina.DepartamentoKey = empleado.DepartamentoKey ?? nomina.DepartamentoKey;
            nomina.HorasTrabajadas = nomina.QTYHorasRegulares;
            nomina.HorasExtra = nomina.QTYHorasExtras;
            nomina.PagoHorasRegulares = decimal.Round(horaRegular * nomina.QTYHorasRegulares, 2, MidpointRounding.AwayFromZero);
            nomina.PagoHorasExtra = NominaPlanillaCalculo.PagoHorasExtras(salarioBase, nomina.QTYHorasExtras);
            nomina.Feriados = NominaPlanillaCalculo.MontoFeriados(salarioBase, nomina.QTYDiasFeriados);

            nomina.Bonificaciones = decimal.Round(empleado.Bonos ?? 0m, 2, MidpointRounding.AwayFromZero);
            if (sincronizarMiscelaneoConBonos)
            {
                nomina.Miscelaneo = nomina.Bonificaciones;
            }
            else
            {
                nomina.Miscelaneo = decimal.Round(nomina.Miscelaneo, 2, MidpointRounding.AwayFromZero);
            }

            nomina.Comisiones = 0m;
            nomina.Aguinaldo = 0m;

            var ccss = decimal.Round(empleado.CCSS ?? 0m, 2, MidpointRounding.AwayFromZero);
            var jupema = decimal.Round(empleado.JUPEMA ?? 0m, 2, MidpointRounding.AwayFromZero);
            var magisterio = decimal.Round(empleado.Magisterio ?? 0m, 2, MidpointRounding.AwayFromZero);
            var rebajos = decimal.Round(empleado.Rebajos ?? 0m, 2, MidpointRounding.AwayFromZero);
            var porcentajeBp = decimal.Round(empleado.PorcentajeBP ?? 0m, 2, MidpointRounding.AwayFromZero);

            nomina.SeguroSocial = ccss;
            nomina.Renta = 0m;
            nomina.OtrasDeducciones = jupema + magisterio + rebajos + porcentajeBp;
            nomina.OtrasDeducciones = decimal.Round(nomina.OtrasDeducciones, 2, MidpointRounding.AwayFromZero);
            nomina.ExtrasQuincenales = decimal.Round(nomina.ExtrasQuincenales, 2, MidpointRounding.AwayFromZero);
            nomina.DeduccionesQuincenales = decimal.Round(nomina.DeduccionesQuincenales, 2, MidpointRounding.AwayFromZero);

            nomina.TotalExtras = NominaPlanillaCalculo.Credito(nomina);
            nomina.TotalDeducciones = NominaPlanillaCalculo.Debito(nomina);
            nomina.SalarioNeto = NominaPlanillaCalculo.SalarioNeto(salarioBase, nomina);
        }

        public async Task<decimal> GetTotalNominaAsync(int ano, int mes, int quincena, string? sede)
        {
            int diaQuincena = quincena == 1 ? 15 : DateTime.DaysInMonth(ano, mes);
            var periodoKey = ano * 10000 + mes * 100 + diaQuincena;

            var query = _context.Nominas.Where(n => n.PeriodoKey == periodoKey);
            
            if (!string.IsNullOrEmpty(sede))
            {
                query = query.Where(n => n.Sede == sede);
            }

            return await query.SumAsync(n => n.SalarioNeto);
        }

        public async Task<IEnumerable<Nomina>> GetHistorialEmpleadoAsync(int empleadoKey, int top = 12)
        {
            return await _context.Nominas
                .Include(n => n.Puesto)
                .Include(n => n.Departamento)
                .Where(n => n.EmpleadoKey == empleadoKey)
                .OrderByDescending(n => n.PeriodoKey)
                .Take(top)
                .ToListAsync();
        }

        public async Task<List<AguinaldoEmpleadoViewModel>> CalcularAguinaldoAsync(int ano, string? sede)
        {
            var fechaInicio = new DateTime(ano - 1, 12, 1);
            var fechaFin = new DateTime(ano, 11, DateTime.DaysInMonth(ano, 11));
            var periodoInicioKey = fechaInicio.Year * 10000 + fechaInicio.Month * 100 + 1;
            var periodoFinKey = fechaFin.Year * 10000 + fechaFin.Month * 100 + fechaFin.Day;

            var empleadosQuery = _context.Empleados
                .Where(e => e.Activo || (e.FechaSalida.HasValue && e.FechaSalida.Value >= fechaInicio));

            if (!string.IsNullOrWhiteSpace(sede))
            {
                empleadosQuery = empleadosQuery.Where(e => e.Sede == sede);
            }

            var empleados = await empleadosQuery
                .OrderBy(e => e.NombreCompleto)
                .ToListAsync();

            var resultados = new List<AguinaldoEmpleadoViewModel>();

            foreach (var empleado in empleados)
            {
                var nominasPeriodo = await _context.Nominas
                    .Where(n => n.EmpleadoKey == empleado.EmpleadoKey
                        && n.PeriodoKey >= periodoInicioKey
                        && n.PeriodoKey <= periodoFinKey)
                    .ToListAsync();

                decimal aguinaldoCalculado;
                decimal totalDevengadoPeriodo;
                int mesesTrabajados;
                decimal promedioHorasExtraMensual = 0m;
                bool usoCalculoAlternativo = false;
                string detalleCalculo;

                if (nominasPeriodo.Any())
                {
                    totalDevengadoPeriodo = nominasPeriodo.Sum(n =>
                        NominaPlanillaCalculo.SalarioQuincenal(n.SalarioBase) + NominaPlanillaCalculo.Credito(n));

                    aguinaldoCalculado = totalDevengadoPeriodo / 12m;
                    // PeriodoKey = yyyyMMdd → clave única por mes calendario (año*100 + mes)
                    mesesTrabajados = nominasPeriodo
                        .Select(n => (n.PeriodoKey / 10000) * 100 + (n.PeriodoKey / 100) % 100)
                        .Distinct()
                        .Count();

                    detalleCalculo = "Cálculo estándar (01 dic - 30 nov) con nóminas del período.";
                }
                else
                {
                    usoCalculoAlternativo = true;
                    var fechaIngresoAjustada = empleado.FechaIngreso > fechaInicio ? empleado.FechaIngreso : fechaInicio;
                    var fechaSalidaAjustada = empleado.FechaSalida.HasValue && empleado.FechaSalida.Value < fechaFin
                        ? empleado.FechaSalida.Value
                        : fechaFin;

                    if (fechaIngresoAjustada > fechaSalidaAjustada)
                    {
                        continue;
                    }

                    mesesTrabajados = ((fechaSalidaAjustada.Year - fechaIngresoAjustada.Year) * 12) + fechaSalidaAjustada.Month - fechaIngresoAjustada.Month + 1;

                    var historialNomina = await _context.Nominas
                        .Where(n => n.EmpleadoKey == empleado.EmpleadoKey)
                        .ToListAsync();

                    promedioHorasExtraMensual = historialNomina.Any()
                        ? historialNomina.Average(n => n.PagoHorasExtra)
                        : 0m;

                    var salarioBrutoMensual = empleado.SalarioBase ?? 0m;
                    var baseMensualConExtras = salarioBrutoMensual + promedioHorasExtraMensual;
                    totalDevengadoPeriodo = baseMensualConExtras * mesesTrabajados;
                    aguinaldoCalculado = totalDevengadoPeriodo / 12m;

                    detalleCalculo = "Cálculo alternativo por meses trabajados y nóminas ingresadas (salario bruto mensual + promedio de horas extra).";
                }

                resultados.Add(new AguinaldoEmpleadoViewModel
                {
                    EmpleadoKey = empleado.EmpleadoKey,
                    NombreEmpleado = empleado.NombreCompleto ?? $"{empleado.Nombre} {empleado.Apellido1}".Trim(),
                    Sede = empleado.Sede,
                    SalarioBase = empleado.SalarioBase ?? 0m,
                    MesesTrabajadosPeriodo = mesesTrabajados,
                    NominasConsideradas = nominasPeriodo.Count,
                    TotalDevengadoPeriodo = totalDevengadoPeriodo,
                    PromedioHorasExtraMensual = promedioHorasExtraMensual,
                    AguinaldoCalculado = Math.Round(aguinaldoCalculado, 2),
                    SeUsoCalculoAlternativo = usoCalculoAlternativo,
                    DetalleCalculo = detalleCalculo
                });
            }

            return resultados;
        }
    }
}
