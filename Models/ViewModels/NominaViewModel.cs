using System.ComponentModel.DataAnnotations;

namespace HRM.SantaLucia.Web.Models.ViewModels
{
    public class NominaViewModel
    {
        public int NominaKey { get; set; }
        public int EmpleadoKey { get; set; }
        public string NombreEmpleado { get; set; }
        public string NombrePuesto { get; set; }
        public string NombreDepartamento { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; } // Para quincena
        public string NombreMes { get; set; }
        public string? Sede { get; set; }
        public string Quincena { get; set; } // Primera o Segunda

        [Display(Name = "Salario Base")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal SalarioBase { get; set; }

        [Display(Name = "Total Extras")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal TotalExtras { get; set; }

        [Display(Name = "Total Deducciones")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal TotalDeducciones { get; set; }

        [Display(Name = "Salario Neto")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal SalarioNeto { get; set; }

        [Display(Name = "Horas Trabajadas")]
        public decimal HorasTrabajadas { get; set; }

        [Display(Name = "Horas Extra")]
        public decimal HorasExtra { get; set; }

        [Display(Name = "Pago Horas Regulares")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal PagoHorasRegulares { get; set; }

        // Campos editables en nómina
        [Display(Name = "QTY Horas Extras")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal QTYHorasExtras { get; set; }

        [Display(Name = "QTY Horas Regulares")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal QTYHorasRegulares { get; set; }

        [Display(Name = "Misceláneo")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Miscelaneo { get; set; }

        [Display(Name = "Días Feriados")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal QTYDiasFeriados { get; set; }

        [Display(Name = "Feriados (monto)")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Feriados { get; set; }

        [Display(Name = "Aguinaldo")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Aguinaldo { get; set; }

        [Display(Name = "Extras Quincenales")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal ExtrasQuincenales { get; set; }

        [Display(Name = "Deducciones Quincenales")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal DeduccionesQuincenales { get; set; }
    }

    public class CalcularNominaViewModel
    {
        [Display(Name = "Año")]
        [Required(ErrorMessage = "El año es requerido")]
        [Range(2020, 2030, ErrorMessage = "Año inválido")]
        public int Ano { get; set; }

        [Display(Name = "Mes")]
        [Required(ErrorMessage = "El mes es requerido")]
        [Range(1, 12, ErrorMessage = "Mes inválido")]
        public int Mes { get; set; }

        [Display(Name = "Quincena")]
        [Required(ErrorMessage = "La quincena es requerida")]
        [Range(1, 2, ErrorMessage = "Quincena inválida (1 o 2)")]
        public int Quincena { get; set; } // 1 = Primera quincena (1-15), 2 = Segunda quincena (16-fin de mes)

        [Display(Name = "Sede")]
        [Required(ErrorMessage = "La sede es requerida")]
        public string Sede { get; set; } // Kamakiri o Complejo Educativo

        public List<NominaViewModel> Nominas { get; set; }
        public decimal TotalAPagar { get; set; }
        public int TotalEmpleados { get; set; }
    }

    public class CalcularAguinaldoViewModel
    {
        [Display(Name = "Año")]
        [Required(ErrorMessage = "El año es requerido")]
        [Range(2020, 2035, ErrorMessage = "Año inválido")]
        public int Ano { get; set; }

        [Display(Name = "Sede")]
        public string? Sede { get; set; }

        public DateTime FechaInicioPeriodo { get; set; }
        public DateTime FechaFinPeriodo { get; set; }
        public List<AguinaldoEmpleadoViewModel> Resultados { get; set; } = new();
        public decimal TotalAguinaldo { get; set; }
    }

    public class AguinaldoEmpleadoViewModel
    {
        public int EmpleadoKey { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public string? Sede { get; set; }
        public decimal SalarioBase { get; set; }
        public int MesesTrabajadosPeriodo { get; set; }
        public int NominasConsideradas { get; set; }
        public decimal TotalDevengadoPeriodo { get; set; }
        public decimal PromedioHorasExtraMensual { get; set; }
        public decimal AguinaldoCalculado { get; set; }
        public bool SeUsoCalculoAlternativo { get; set; }
        public string DetalleCalculo { get; set; } = string.Empty;
    }

    public class EditarNominaViewModel
    {
        public int NominaKey { get; set; }
        public int EmpleadoKey { get; set; }
        public string? NombreEmpleado { get; set; }
        public decimal SalarioBase { get; set; }

        [Display(Name = "QTY Horas Extras")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal QTYHorasExtras { get; set; }

        [Display(Name = "QTY Horas Regulares")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal QTYHorasRegulares { get; set; }

        [Display(Name = "Días Feriados")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal QTYDiasFeriados { get; set; }

        [Display(Name = "Misceláneo")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Miscelaneo { get; set; }

        [Display(Name = "Extras Quincenales")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal ExtrasQuincenales { get; set; }

        [Display(Name = "Deducciones Quincenales")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal DeduccionesQuincenales { get; set; }

        /// <summary>Mes del periodo (1-12) para calcular aguinaldo automático.</summary>
        public int MesPeriodo { get; set; }
    }
}
