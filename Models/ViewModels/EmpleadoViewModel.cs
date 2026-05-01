using System.ComponentModel.DataAnnotations;

namespace HRM.SantaLucia.Web.Models.ViewModels
{
    public class EmpleadoViewModel
    {
        public int EmpleadoKey { get; set; }

        [Display(Name = "C�digo Empleado")]
        [Required(ErrorMessage = "El c�digo de empleado es requerido")]
        public string EmpleadoID { get; set; }

        [Display(Name = "C�dula")]
        [Required(ErrorMessage = "La c�dula es requerida")]
        public string Cedula { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Primer Apellido")]
        [Required(ErrorMessage = "El primer apellido es requerido")]
        public string Apellido1 { get; set; }

        [Display(Name = "Segundo Apellido")]
        public string? Apellido2 { get; set; }

        [Display(Name = "Correo Electr�nico")]
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Correo electr�nico inv�lido")]
        public string Email { get; set; }

        [Display(Name = "Correo Personal")]
        [EmailAddress(ErrorMessage = "Correo electr�nico inv�lido")]
        public string? EmailPersonal { get; set; }

        [Display(Name = "Tel�fono")]
        [Required(ErrorMessage = "El tel�fono es requerido")]
        [Phone(ErrorMessage = "Tel�fono inv�lido")]
        public string Telefono { get; set; }

        [Display(Name = "Tel�fono Emergencia")]
        [Phone(ErrorMessage = "Tel�fono inv�lido")]
        public string? TelefonoEmergencia { get; set; }

        [Display(Name = "Contacto Emergencia")]
        public string? ContactoEmergencia { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "G�nero")]
        [Required(ErrorMessage = "El g�nero es requerido")]
        public string Genero { get; set; }

        [Display(Name = "Estado Civil")]
        public string? EstadoCivil { get; set; }

        [Display(Name = "Nacionalidad")]
        [Required(ErrorMessage = "La nacionalidad es requerida")]
        public string Nacionalidad { get; set; }

        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "La provincia es requerida")]
        public string Provincia { get; set; }

        [Display(Name = "Cant�n")]
        [Required(ErrorMessage = "El cant�n es requerido")]
        public string Canton { get; set; }

        [Display(Name = "Distrito")]
        [Required(ErrorMessage = "El distrito es requerido")]
        public string Distrito { get; set; }

        [Display(Name = "Direcci�n Exacta")]
        [Required(ErrorMessage = "La direcci�n es requerida")]
        public string DireccionExacta { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        [Required(ErrorMessage = "La fecha de ingreso es requerida")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Display(Name = "Fecha de Salida")]
        [DataType(DataType.Date)]
        public DateTime? FechaSalida { get; set; }

        [Display(Name = "Tipo de Contrato")]
        [Required(ErrorMessage = "El tipo de contrato es requerido")]
        public string TipoContrato { get; set; }

        [Display(Name = "Puesto")]
        public int? PuestoKey { get; set; }

        [Display(Name = "Departamento")]
        public int? DepartamentoKey { get; set; }

        [Display(Name = "Cuenta Bancaria")]
        public string? CuentaBancaria { get; set; }

        [Display(Name = "Banco")]
        public int? BancoKey { get; set; }

        [Display(Name = "Nivel Educativo")]
        public string? NivelEducativo { get; set; }

        public string? Foto { get; set; }

        [Display(Name = "Sede")]
        public string? Sede { get; set; }

        // Campos salariales editables
        [Display(Name = "Salario Base")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? SalarioBase { get; set; }

        [Display(Name = "Salario Neto")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? SalarioNeto { get; set; }

        [Display(Name = "Rebajos")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? Rebajos { get; set; }

        [Display(Name = "CCSS")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? CCSS { get; set; }

        [Display(Name = "JUPEMA")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? JUPEMA { get; set; }

        [Display(Name = "Magisterio")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? Magisterio { get; set; }

        [Display(Name = "Monto BP (1% del BP)")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? PorcentajeBP { get; set; }

        [Display(Name = "Bonos")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? Bonos { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        // Propiedades de solo lectura para display
        public string? NombreCompleto { get; set; }
        public int Edad { get; set; }
        public string? NombrePuesto { get; set; }
        public string? NombreDepartamento { get; set; }
        public string? NombreBanco { get; set; }
    }

    public class EmpleadoListViewModel
    {
        public List<EmpleadoViewModel> Empleados { get; set; }
        public string SearchTerm { get; set; }
        public int? DepartamentoFilter { get; set; }
        public bool SoloActivos { get; set; } = true;
        public int TotalRegistros { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }
}