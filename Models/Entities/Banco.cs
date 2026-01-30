using System.ComponentModel.DataAnnotations;

namespace HRM.SantaLucia.Web.Models.Entities
{
    public class Banco
    {
        [Key]
        public int BancoKey { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreBanco { get; set; }

        [StringLength(20)]
        public string? CodigoBanco { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        [StringLength(100)]
        public string? UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [StringLength(100)]
        public string? UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}

