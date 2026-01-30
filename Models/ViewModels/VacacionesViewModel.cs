using System.ComponentModel.DataAnnotations;

namespace HRM.SantaLucia.Web.Models.ViewModels
{
    public class VacacionesViewModel
    {
        public int VacacionKey { get; set; }

        [Display(Name = "Empleado")]
        [Required(ErrorMessage = "Debe seleccionar un empleado")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un empleado")]
        public int EmpleadoKey { get; set; }

        [Display(Name = "Fecha de Inicio")]
        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Fecha de Fin")]
        [Required(ErrorMessage = "La fecha de fin es requerida")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        [Display(Name = "D�as Solicitados")]
        [Required(ErrorMessage = "Debe indicar los d�as solicitados")]
        [Range(0, 365, ErrorMessage = "Los d�as deben estar entre 1 y 365")]
        public int DiasSolicitados { get; set; } // Se calcula en el controlador desde FechaInicio/FechaFin

        [Display(Name = "D�as Disponibles")]
        public int DiasDisponibles { get; set; }

        [Display(Name = "Estado")]
        public string? Estado { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(500)]
        public string? Observaciones { get; set; }

        // Propiedades de solo lectura
        public string? NombreEmpleado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string? NombreAprobador { get; set; }
    }

    public class VacacionesPendientesViewModel
    {
        public List<VacacionesViewModel> Solicitudes { get; set; }
        public int TotalPendientes { get; set; }
    }
}