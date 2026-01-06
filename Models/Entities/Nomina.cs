using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.SantaLucia.Web.Models.Entities
{
    public class Nomina
    {
        [Key]
        public int NominaKey { get; set; }

        [Required]
        public int EmpleadoKey { get; set; }
        [ForeignKey("EmpleadoKey")]
        public virtual Empleado Empleado { get; set; }

        [Required]
        public int PuestoKey { get; set; }
        [ForeignKey("PuestoKey")]
        public virtual Puesto Puesto { get; set; }

        [Required]
        public int DepartamentoKey { get; set; }
        [ForeignKey("DepartamentoKey")]
        public virtual Departamento Departamento { get; set; }

        [Required]
        public int PeriodoKey { get; set; } // Formato: yyyyMM

        [Column(TypeName = "decimal(18,2)")]
        public decimal SalarioBase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HorasTrabajadas { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HorasExtra { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PagoHorasExtra { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Bonificaciones { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Comisiones { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalExtras { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SeguroSocial { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Renta { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OtrasDeducciones { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDeducciones { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SalarioNeto { get; set; }

        [StringLength(100)]
        public string? UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [StringLength(100)]
        public string? UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}

