using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.SantaLucia.Web.Models.Entities
{
    public class Empleado
    {
        [Key]
        public int EmpleadoKey { get; set; }

        [Required]
        [StringLength(20)]
        public string EmpleadoID { get; set; }

        [Required]
        [StringLength(20)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido1 { get; set; }

        [StringLength(100)]
        public string? Apellido2 { get; set; }

        [StringLength(200)]
        public string? NombreCompleto { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? EmailPersonal { get; set; }

        [Required]
        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(20)]
        public string? TelefonoEmergencia { get; set; }

        [StringLength(200)]
        public string? ContactoEmergencia { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        public int? Edad { get; set; }

        [Required]
        [StringLength(20)]
        public string Genero { get; set; }

        [StringLength(20)]
        public string? EstadoCivil { get; set; }

        [Required]
        [StringLength(50)]
        public string Nacionalidad { get; set; }

        [Required]
        [StringLength(50)]
        public string Provincia { get; set; }

        [Required]
        [StringLength(50)]
        public string Canton { get; set; }

        [Required]
        [StringLength(50)]
        public string Distrito { get; set; }

        [Required]
        [StringLength(500)]
        public string DireccionExacta { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaSalida { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoContrato { get; set; }

        public int? PuestoKey { get; set; }
        [ForeignKey("PuestoKey")]
        public virtual Puesto? Puesto { get; set; }

        public int? DepartamentoKey { get; set; }
        [ForeignKey("DepartamentoKey")]
        public virtual Departamento? Departamento { get; set; }

        [StringLength(50)]
        public string? CuentaBancaria { get; set; }

        public int? BancoKey { get; set; }
        [ForeignKey("BancoKey")]
        public virtual Banco? Banco { get; set; }

        [StringLength(50)]
        public string? NivelEducativo { get; set; }

        [StringLength(500)]
        public string? Foto { get; set; }

        public bool Activo { get; set; } = true;

        [StringLength(100)]
        public string? UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [StringLength(100)]
        public string? UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}

