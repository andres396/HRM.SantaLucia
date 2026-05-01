namespace HRM.SantaLucia.Web.Helpers
{
    /// <summary>
    /// Edad a partir de la fecha de nacimiento (equivalente a la lógica de la columna calculada en SQL).
    /// </summary>
    public static class EmpleadoEdad
    {
        public static int Calcular(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > hoy.AddYears(-edad))
                edad--;
            return edad;
        }
    }
}
