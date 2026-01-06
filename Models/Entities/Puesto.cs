using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.SantaLucia.Web.Models.Entities
{
    public class Puesto
    {
        [Key]
        public int PuestoKey { get; set; }

        [Required]
        [StringLength(100)]
        public string NombrePuesto { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalarioMinimo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalarioMaximo { get; set; }

        [StringLength(50)]
        public string? TipoPuesto { get; set; } // Docente, Administrativo, etc.

        public bool Activo { get; set; } = true;

        [StringLength(100)]
        public string? UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [StringLength(100)]
        public string? UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        // Navigation property
        public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}

