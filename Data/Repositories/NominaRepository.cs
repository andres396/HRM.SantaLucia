using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using HRM.SantaLucia.Web.Data;
using HRM.SantaLucia.Web.Models.Entities;

namespace HRM.SantaLucia.Web.Data.Repositories
{
    public class NominaRepository : INominaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public NominaRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            // Usar la cadena de conexión directamente sin modificaciones
            // El DbContext ya tiene la cadena configurada correctamente
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Nomina>> GetByPeriodoAsync(int ano, int mes, int quincena, string? sede)
        {
            // Calcular el día de la quincena
            int diaQuincena = quincena == 1 ? 15 : DateTime.DaysInMonth(ano, mes);
            var periodoKey = ano * 10000 + mes * 100 + diaQuincena;

            var query = _context.Nominas
                .Include(n => n.Empleado)
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
                        var salarioBase = empleado.SalarioBase ?? empleado.Puesto?.SalarioMinimo ?? 0;
                        var qtyHorasRegulares = 80m; // 80 horas por quincena (default)
                        var qtyHorasExtras = 0m;
                        var qtyDiasFeriados = 0m;

                        // Fórmulas: SalarioBrutoDiario = SalarioBase/30, HoraRegular = SalarioBrutoDiario/8 = SalarioBase/240
                        var salarioBrutoDiario = salarioBase / 30m;
                        var horaRegular = salarioBrutoDiario / 8m;
                        var pagoHorasRegulares = horaRegular * qtyHorasRegulares;
                        var pagoHorasExtra = horaRegular * 1.5m * qtyHorasExtras;
                        var feriadosMonto = salarioBrutoDiario * 2m * qtyDiasFeriados;

                        var bonificaciones = empleado.Bonos ?? 0;
                        // Aguinaldo automático: solo en diciembre = 1/12 del salario
                        var aguinaldo = (mes == 12) ? (salarioBase / 12m) : 0m;
                        var totalExtras = pagoHorasExtra + feriadosMonto + bonificaciones + aguinaldo;

                        var ccss = empleado.CCSS ?? 0;
                        var jupema = empleado.JUPEMA ?? 0;
                        var magisterio = empleado.Magisterio ?? 0;
                        var rebajos = empleado.Rebajos ?? 0;
                        var porcentajeBP = empleado.PorcentajeBP ?? 0;
                        var totalDeducciones = ccss + jupema + magisterio + rebajos + (salarioBase * (porcentajeBP / 100m));
                        var salarioNeto = pagoHorasRegulares + totalExtras - totalDeducciones;

                        var nomina = new Nomina
                        {
                            EmpleadoKey = empleado.EmpleadoKey,
                            PuestoKey = empleado.PuestoKey.Value,
                            DepartamentoKey = empleado.DepartamentoKey.Value,
                            PeriodoKey = periodoKey,
                            Sede = sede,
                            SalarioBase = salarioBase,
                            HorasTrabajadas = qtyHorasRegulares,
                            HorasExtra = qtyHorasExtras,
                            PagoHorasRegulares = pagoHorasRegulares,
                            PagoHorasExtra = pagoHorasExtra,
                            Bonificaciones = bonificaciones,
                            Comisiones = 0,
                            TotalExtras = totalExtras,
                            SeguroSocial = ccss,
                            Renta = 0,
                            OtrasDeducciones = jupema + magisterio + rebajos,
                            TotalDeducciones = totalDeducciones,
                            SalarioNeto = salarioNeto,
                            Miscelaneo = 0,
                            QTYDiasFeriados = qtyDiasFeriados,
                            Feriados = feriadosMonto,
                            Aguinaldo = aguinaldo,
                            ExtrasQuincenales = 0,
                            DeduccionesQuincenales = 0,
                            QTYHorasRegulares = qtyHorasRegulares,
                            QTYHorasExtras = qtyHorasExtras,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = usuarioCreacion
                        };

                        _context.Nominas.Add(nomina);
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

                // Actualizar campos editables
                existing.QTYHorasExtras = nomina.QTYHorasExtras;
                existing.QTYHorasRegulares = nomina.QTYHorasRegulares;
                existing.QTYDiasFeriados = nomina.QTYDiasFeriados;
                existing.Miscelaneo = nomina.Miscelaneo;
                existing.ExtrasQuincenales = nomina.ExtrasQuincenales;
                existing.DeduccionesQuincenales = nomina.DeduccionesQuincenales;

                // Aguinaldo automático: en diciembre = 1 mes de salario (SalarioBase/12), resto del año = 0
                var mes = (existing.PeriodoKey / 100) % 100;
                existing.Aguinaldo = (mes == 12) ? (existing.SalarioBase / 12m) : 0m;

                // Fórmulas: HoraRegular = SalarioBase/240, PagoHorasExtra = HoraRegular*1.5*QTYHorasExtras, Feriados = SalarioBrutoDiario*2*QTYDiasFeriados
                var salarioBrutoDiario = existing.SalarioBase / 30m;
                var horaRegular = salarioBrutoDiario / 8m;
                existing.PagoHorasRegulares = horaRegular * existing.QTYHorasRegulares;
                existing.PagoHorasExtra = horaRegular * 1.5m * existing.QTYHorasExtras;
                existing.Feriados = salarioBrutoDiario * 2m * existing.QTYDiasFeriados;
                existing.HorasExtra = existing.QTYHorasExtras;

                existing.TotalExtras = existing.PagoHorasExtra + existing.Feriados + existing.Bonificaciones + existing.Comisiones + existing.Miscelaneo + existing.Aguinaldo + existing.ExtrasQuincenales;
                existing.TotalDeducciones = existing.SeguroSocial + existing.Renta + existing.OtrasDeducciones + existing.DeduccionesQuincenales;
                existing.SalarioNeto = existing.PagoHorasRegulares + existing.TotalExtras - existing.TotalDeducciones;

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
    }
}
