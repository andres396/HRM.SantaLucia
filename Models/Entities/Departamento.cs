using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.SantaLucia.Web.Models.Entities
{
    public class Departamento
    {
        [Key]
        public int DepartamentoKey { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreDepartamento { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public int? DepartamentoPadreKey { get; set; }
        [ForeignKey("DepartamentoPadreKey")]
        public virtual Departamento? DepartamentoPadre { get; set; }

        public int? JefeKey { get; set; }
        [ForeignKey("JefeKey")]
        public virtual Empleado? Jefe { get; set; }

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

