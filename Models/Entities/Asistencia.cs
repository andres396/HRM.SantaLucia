using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.SantaLucia.Web.Models.Entities
{
    public class Asistencia
    {
        [Key]
        public int AsistenciaKey { get; set; }

        [Required]
        public int EmpleadoKey { get; set; }
        [ForeignKey("EmpleadoKey")]
        public virtual Empleado Empleado { get; set; }

        [Required]
        public int FechaKey { get; set; } // Formato: yyyyMMdd

        public TimeSpan? HoraEntrada { get; set; }

        public TimeSpan? HoraSalida { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal HorasTrabajadas { get; set; }

        public int MinutosTarde { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } // Presente, Tarde, Ausente, Justificado, Permiso

        [StringLength(500)]
        public string? Justificacion { get; set; }

        public bool GoceSalario { get; set; } = false;

        [StringLength(500)]
        public string? MotivoPermiso { get; set; }

        [StringLength(100)]
        public string? UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [StringLength(100)]
        public string? UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}

