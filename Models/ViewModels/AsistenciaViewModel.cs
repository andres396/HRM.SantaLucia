using System.ComponentModel.DataAnnotations;

namespace HRM.SantaLucia.Web.Models.ViewModels
{
    public class AsistenciaViewModel
    {
        public int AsistenciaKey { get; set; }

        [Display(Name = "Empleado")]
        [Required(ErrorMessage = "Debe seleccionar un empleado")]
        public int EmpleadoKey { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "La fecha es requerida")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Hora de Entrada")]
        [DataType(DataType.Time)]
        public TimeSpan? HoraEntrada { get; set; }

        [Display(Name = "Hora de Salida")]
        [DataType(DataType.Time)]
        public TimeSpan? HoraSalida { get; set; }

        [Display(Name = "Horas Trabajadas")]
        public decimal HorasTrabajadas { get; set; }

        [Display(Name = "Minutos de Retraso")]
        public int MinutosTarde { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El estado es requerido")]
        public string Estado { get; set; }

        [Display(Name = "Justificaci�n")]
        public string? Justificacion { get; set; }

        [Display(Name = "Goce de Salario")]
        public bool GoceSalario { get; set; } = false;

        [Display(Name = "Motivo del Permiso")]
        [StringLength(500)]
        public string? MotivoPermiso { get; set; }

        // Propiedades de solo lectura
        public string? NombreEmpleado { get; set; }
        public string? DepartamentoEmpleado { get; set; }
    }

    public class AsistenciaDiariaViewModel
    {
        public DateTime Fecha { get; set; }
        public List<AsistenciaViewModel> Asistencias { get; set; }
        public int TotalPresentes { get; set; }
        public int TotalAusentes { get; set; }
        public int TotalTarde { get; set; }
    }
}