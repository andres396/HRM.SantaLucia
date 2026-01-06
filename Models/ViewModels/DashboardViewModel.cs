using System;
using System.Collections.Generic;

namespace HRM.SantaLucia.Web.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalEmpleadosActivos { get; set; }
        public int TotalDocentes { get; set; }
        public int TotalAdministrativos { get; set; }
        public decimal TotalNominaMes { get; set; }
        public decimal PromedioSalario { get; set; }
        public int VacacionesPendientes { get; set; }
        public int AsistenciaHoy { get; set; }
        public List<ProximoCumpleanosDto> ProximosCumpleanos { get; set; }
        public List<NominaResumenDto> ResumenNominaMensual { get; set; }
    }

    public class ProximoCumpleanosDto
    {
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int DiasHastaCumpleanos { get; set; }
    }

    public class NominaResumenDto
    {
        public string Departamento { get; set; }
        public decimal Total { get; set; }
        public int NumeroEmpleados { get; set; }
    }
}
