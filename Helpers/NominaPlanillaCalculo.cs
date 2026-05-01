using HRM.SantaLucia.Web.Models.Entities;

namespace HRM.SantaLucia.Web.Helpers
{
    /// <summary>
    /// Cálculo único de planilla quincenal (planilla). Credito y Debito solo se exponen en exportaciones (CSV/Excel).
    /// </summary>
    public static class NominaPlanillaCalculo
    {
        /// <summary>Salario mensual bruto entre dos quincenas.</summary>
        public static decimal SalarioQuincenal(decimal salarioBaseMensual) =>
            decimal.Round(salarioBaseMensual / 2m, 2, MidpointRounding.AwayFromZero);

        /// <summary>Hora regular según salario mensual / 30 / 8.</summary>
        public static decimal ValorHoraRegular(decimal salarioBaseMensual)
        {
            var salarioDiario = salarioBaseMensual / 30m;
            return salarioDiario / 8m;
        }

        public static decimal PagoHorasExtras(decimal salarioBaseMensual, decimal qtyHorasExtras)
        {
            var horaRegular = ValorHoraRegular(salarioBaseMensual);
            return decimal.Round(horaRegular * 1.5m * qtyHorasExtras, 2, MidpointRounding.AwayFromZero);
        }

        public static decimal MontoFeriados(decimal salarioBaseMensual, decimal qtyDiasFeriados)
        {
            var salarioDiario = salarioBaseMensual / 30m;
            return decimal.Round(salarioDiario * 2m * qtyDiasFeriados, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Todo lo que suma al salario quincenal base (excepto ese base).
        /// Bonificación en planilla: si Misceláneo tiene valor se usa como línea autorizada en planilla;
        /// si está en cero se usa Bonificaciones (ej. sincronizada desde expediente).
        /// </summary>
        public static decimal Credito(Nomina n)
        {
            var bonificacionCredito = n.Miscelaneo != 0m ? n.Miscelaneo : n.Bonificaciones;
            var suma = n.PagoHorasExtra
                + n.Feriados
                + bonificacionCredito
                + n.Comisiones
                + n.ExtrasQuincenales
                + n.Aguinaldo;
            return decimal.Round(suma, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>Todo lo que resta al salario quincenal base.</summary>
        public static decimal Debito(Nomina n)
        {
            var suma = n.SeguroSocial + n.Renta + n.OtrasDeducciones + n.DeduccionesQuincenales;
            return decimal.Round(suma, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Neto quincenal: salario/2 − débitos + créditos.
        /// </summary>
        public static decimal SalarioNeto(decimal salarioBaseMensual, Nomina n)
        {
            var baseQ = SalarioQuincenal(salarioBaseMensual);
            var c = Credito(n);
            var d = Debito(n);
            return decimal.Round(baseQ - d + c, 2, MidpointRounding.AwayFromZero);
        }
    }
}
