using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.SantaLucia.Web.Models.Entities
{
    public class Vacaciones
    {
        [Key]
        public int VacacionKey { get; set; }

        [Required]
        public int EmpleadoKey { get; set; }
        [ForeignKey("EmpleadoKey")]
        public virtual Empleado Empleado { get; set; }

        [Required]
        public int FechaInicioKey { get; set; } // Formato: yyyyMMdd

        [Required]
        public int FechaFinKey { get; set; } // Formato: yyyyMMdd

        [Required]
        public int DiasSolicitados { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } // Pendiente, Aprobada, Rechazada, Cancelada

        [StringLength(500)]
        public string? Observaciones { get; set; }

        public int? AprobadorKey { get; set; }
        [ForeignKey("AprobadorKey")]
        public virtual Empleado? Aprobador { get; set; }

        public DateTime? FechaSolicitud { get; set; }

        public DateTime? FechaAprobacion { get; set; }

        [StringLength(100)]
        public string? UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [StringLength(100)]
        public string? UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}

