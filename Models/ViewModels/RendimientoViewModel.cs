using System.ComponentModel.DataAnnotations;

namespace HRM.SantaLucia.Web.Models.ViewModels
{
    public class RendimientoViewModel
    {
        public int RendimientoKey { get; set; }

        [Display(Name = "Empleado")]
        [Required(ErrorMessage = "Debe seleccionar un empleado")]
        public int EmpleadoKey { get; set; }

        [Display(Name = "Evaluador")]
        [Required(ErrorMessage = "Debe seleccionar un evaluador")]
        public int EvaluadorKey { get; set; }

        [Display(Name = "Fecha de Evaluaci�n")]
        [Required(ErrorMessage = "La fecha es requerida")]
        [DataType(DataType.Date)]
        public DateTime FechaEvaluacion { get; set; }

        [Display(Name = "Per�odo")]
        [Required(ErrorMessage = "El per�odo es requerido")]
        public string PeriodoEvaluacion { get; set; }

        [Display(Name = "Conocimiento T�cnico")]
        [Required]
        [Range(0, 5, ErrorMessage = "La calificaci�n debe estar entre 0 y 5")]
        public decimal CalificacionConocimiento { get; set; }

        [Display(Name = "Calidad del Trabajo")]
        [Required]
        [Range(0, 5, ErrorMessage = "La calificaci�n debe estar entre 0 y 5")]
        public decimal CalificacionCalidad { get; set; }

        [Display(Name = "Puntualidad y Asistencia")]
        [Required]
        [Range(0, 5, ErrorMessage = "La calificaci�n debe estar entre 0 y 5")]
        public decimal CalificacionPuntualidad { get; set; }

        [Display(Name = "Trabajo en Equipo")]
        [Required]
        [Range(0, 5, ErrorMessage = "La calificaci�n debe estar entre 0 y 5")]
        public decimal CalificacionTrabajoEquipo { get; set; }

        [Display(Name = "Iniciativa y Proactividad")]
        [Required]
        [Range(0, 5, ErrorMessage = "La calificaci�n debe estar entre 0 y 5")]
        public decimal CalificacionIniciativa { get; set; }

        [Display(Name = "Metas Propuestas")]
        [Required]
        [Range(0, 100, ErrorMessage = "Valor inv�lido")]
        public int MetasPropuestas { get; set; }

        [Display(Name = "Metas Alcanzadas")]
        [Required]
        [Range(0, 100, ErrorMessage = "Valor inv�lido")]
        public int MetasAlcanzadas { get; set; }

        [Display(Name = "Fortalezas")]
        [StringLength(1000)]
        public string? Fortalezas { get; set; }

        [Display(Name = "�reas de Mejora")]
        [StringLength(1000)]
        public string? AreasMejora { get; set; }

        [Display(Name = "Comentarios Generales")]
        [StringLength(2000)]
        public string? Comentarios { get; set; }

        // Propiedades de solo lectura
        public decimal CalificacionGeneral { get; set; }
        public string? NombreEmpleado { get; set; }
        public string? NombreEvaluador { get; set; }
        public decimal PorcentajeCumplimiento { get; set; }
    }
}