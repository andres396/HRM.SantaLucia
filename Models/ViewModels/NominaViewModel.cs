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
        public string NombreMes { get; set; }

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

        public List<NominaViewModel> Nominas { get; set; }
        public decimal TotalAPagar { get; set; }
        public int TotalEmpleados { get; set; }
    }
}
