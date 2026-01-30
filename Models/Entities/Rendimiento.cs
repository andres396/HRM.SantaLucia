using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.SantaLucia.Web.Models.Entities
{
    public class Rendimiento
    {
        [Key]
        public int RendimientoKey { get; set; }

        [Required]
        public int EmpleadoKey { get; set; }
        [ForeignKey("EmpleadoKey")]
        public virtual Empleado Empleado { get; set; }

        [Required]
        public int EvaluadorKey { get; set; }
        [ForeignKey("EvaluadorKey")]
        public virtual Empleado Evaluador { get; set; }

        [Required]
        public int FechaKey { get; set; } // Formato: yyyyMMdd

        [Column(TypeName = "decimal(5,2)")]
        public decimal CalificacionGeneral { get; set; }

        public int MetasPropuestas { get; set; }

        public int MetasAlcanzadas { get; set; }

        [StringLength(1000)]
        public string? Comentarios { get; set; }

        [StringLength(50)]
        public string? PeriodoEvaluacion { get; set; }

        [StringLength(100)]
        public string? UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [StringLength(100)]
        public string? UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}

