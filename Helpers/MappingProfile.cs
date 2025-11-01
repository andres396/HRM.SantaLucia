using AutoMapper;
using HRM.SantaLucia.Web.Models.Entities;
using HRM.SantaLucia.Web.Models.ViewModels;

namespace HRM.SantaLucia.Web.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Empleado Mappings
            CreateMap<Empleado, EmpleadoViewModel>()
                .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.Puesto != null ? src.Puesto.NombrePuesto : null))
                .ForMember(dest => dest.NombreDepartamento, opt => opt.MapFrom(src => src.Departamento != null ? src.Departamento.NombreDepartamento : null))
                .ForMember(dest => dest.NombreBanco, opt => opt.MapFrom(src => src.Banco != null ? src.Banco.NombreBanco : null))
                .ReverseMap();

            // Nómina Mappings
            CreateMap<Nomina, NominaViewModel>()
                .ForMember(dest => dest.NombreEmpleado, opt => opt.MapFrom(src => src.Empleado.NombreCompleto))
                .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.Puesto.NombrePuesto))
                .ForMember(dest => dest.NombreDepartamento, opt => opt.MapFrom(src => src.Departamento.NombreDepartamento))
                .ForMember(dest => dest.Año, opt => opt.Ignore())
                .ForMember(dest => dest.Mes, opt => opt.Ignore())
                .ForMember(dest => dest.NombreMes, opt => opt.Ignore());

            // Asistencia Mappings
            CreateMap<Asistencia, AsistenciaViewModel>()
                .ForMember(dest => dest.NombreEmpleado, opt => opt.MapFrom(src => src.Empleado.NombreCompleto))
                .ForMember(dest => dest.DepartamentoEmpleado, opt => opt.MapFrom(src => src.Empleado.Departamento != null ? src.Empleado.Departamento.NombreDepartamento : null))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => DateTime.ParseExact(src.FechaKey.ToString(), "yyyyMMdd", null)))
                .ReverseMap()
                .ForMember(dest => dest.FechaKey, opt => opt.Ignore());

            // Vacaciones Mappings
            CreateMap<Vacaciones, VacacionesViewModel>()
                .ForMember(dest => dest.NombreEmpleado, opt => opt.MapFrom(src => src.Empleado.NombreCompleto))
                .ForMember(dest => dest.NombreAprobador, opt => opt.MapFrom(src => src.Aprobador != null ? src.Aprobador.NombreCompleto : null))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => DateTime.ParseExact(src.FechaInicioKey.ToString(), "yyyyMMdd", null)))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => DateTime.ParseExact(src.FechaFinKey.ToString(), "yyyyMMdd", null)))
                .ReverseMap()
                .ForMember(dest => dest.FechaInicioKey, opt => opt.Ignore())
                .ForMember(dest => dest.FechaFinKey, opt => opt.Ignore());

            // Rendimiento Mappings
            CreateMap<Rendimiento, RendimientoViewModel>()
                .ForMember(dest => dest.NombreEmpleado, opt => opt.MapFrom(src => src.Empleado.NombreCompleto))
                .ForMember(dest => dest.NombreEvaluador, opt => opt.MapFrom(src => src.Evaluador.NombreCompleto))
                .ForMember(dest => dest.FechaEvaluacion, opt => opt.MapFrom(src => DateTime.ParseExact(src.FechaKey.ToString(), "yyyyMMdd", null)))
                .ForMember(dest => dest.PorcentajeCumplimiento, opt => opt.MapFrom(src =>
                    src.MetasPropuestas > 0 ? (decimal)src.MetasAlcanzadas / src.MetasPropuestas * 100 : 0))
                .ReverseMap()
                .ForMember(dest => dest.FechaKey, opt => opt.Ignore())
                .ForMember(dest => dest.CalificacionGeneral, opt => opt.Ignore());
        }
    }
}