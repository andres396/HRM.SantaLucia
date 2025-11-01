namespace HRM.SantaLucia.Web.Models.ViewModels
{
    public class EmpleadoViewModel
    {
        public int EmpleadoKey { get; set; }

        [Display(Name = "Código Empleado")]
        [Required(ErrorMessage = "El código de empleado es requerido")]
        public string EmpleadoID { get; set; }

        [Display(Name = "Cédula")]
        [Required(ErrorMessage = "La cédula es requerida")]
        public string Cedula { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Primer Apellido")]
        [Required(ErrorMessage = "El primer apellido es requerido")]
        public string Apellido1 { get; set; }

        [Display(Name = "Segundo Apellido")]
        public string? Apellido2 { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Correo electrónico inválido")]
        public string Email { get; set; }

        [Display(Name = "Correo Personal")]
        [EmailAddress(ErrorMessage = "Correo electrónico inválido")]
        public string? EmailPersonal { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Teléfono inválido")]
        public string Telefono { get; set; }

        [Display(Name = "Teléfono Emergencia")]
        [Phone(ErrorMessage = "Teléfono inválido")]
        public string? TelefonoEmergencia { get; set; }

        [Display(Name = "Contacto Emergencia")]
        public string? ContactoEmergencia { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Género")]
        [Required(ErrorMessage = "El género es requerido")]
        public string Genero { get; set; }

        [Display(Name = "Estado Civil")]
        public string? EstadoCivil { get; set; }

        [Display(Name = "Nacionalidad")]
        [Required(ErrorMessage = "La nacionalidad es requerida")]
        public string Nacionalidad { get; set; }

        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "La provincia es requerida")]
        public string Provincia { get; set; }

        [Display(Name = "Cantón")]
        [Required(ErrorMessage = "El cantón es requerido")]
        public string Canton { get; set; }

        [Display(Name = "Distrito")]
        [Required(ErrorMessage = "El distrito es requerido")]
        public string Distrito { get; set; }

        [Display(Name = "Dirección Exacta")]
        [Required(ErrorMessage = "La dirección es requerida")]
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