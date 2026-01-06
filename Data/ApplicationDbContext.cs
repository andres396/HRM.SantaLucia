using Microsoft.EntityFrameworkCore;
using HRM.SantaLucia.Web.Models.Entities;

namespace HRM.SantaLucia.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Dimensiones
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Puesto> Puestos { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Banco> Bancos { get; set; }

        // Hechos
        public DbSet<Nomina> Nominas { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Vacaciones> Vacaciones { get; set; }
        public DbSet<Rendimiento> Rendimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraci�n de Empleado
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.ToTable("Empleado", "DIM");
                entity.HasKey(e => e.EmpleadoKey);

                entity.Property(e => e.EmpleadoKey)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NombreCompleto)
                    .HasComputedColumnSql("[Nombre] + ' ' + [Apellido1] + ' ' + ISNULL([Apellido2], '')", stored: true);

                entity.Property(e => e.Edad)
                    .HasComputedColumnSql("(DATEDIFF(YEAR, [FechaNacimiento], GETDATE()) - CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, [FechaNacimiento], GETDATE()), [FechaNacimiento]) > GETDATE() THEN 1 ELSE 0 END)", stored: true);

                entity.HasOne(e => e.Puesto)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(e => e.PuestoKey)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasOne(e => e.Departamento)
                    .WithMany(d => d.Empleados)
                    .HasForeignKey(e => e.DepartamentoKey)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasOne(e => e.Banco)
                    .WithMany()
                    .HasForeignKey(e => e.BancoKey)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);
            });

            // Configuraci�n de Puesto
            modelBuilder.Entity<Puesto>(entity =>
            {
                entity.ToTable("Puesto", "DIM");
                entity.HasKey(p => p.PuestoKey);

                entity.Property(p => p.SalarioMinimo)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.SalarioMaximo)
                    .HasColumnType("decimal(18,2)");
            });

            // Configuraci�n de Departamento
            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.ToTable("Departamento", "DIM");
                entity.HasKey(d => d.DepartamentoKey);

                entity.HasOne(d => d.DepartamentoPadre)
                    .WithMany()
                    .HasForeignKey(d => d.DepartamentoPadreKey)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Jefe)
                    .WithMany()
                    .HasForeignKey(d => d.JefeKey)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuraci�n de Banco
            modelBuilder.Entity<Banco>(entity =>
            {
                entity.ToTable("Banco", "DIM");
                entity.HasKey(b => b.BancoKey);
            });

            // Configuraci�n de N�mina
            modelBuilder.Entity<Nomina>(entity =>
            {
                entity.ToTable("Nomina", "FACT");
                entity.HasKey(n => n.NominaKey);

                entity.HasOne(n => n.Empleado)
                    .WithMany()
                    .HasForeignKey(n => n.EmpleadoKey)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.Puesto)
                    .WithMany()
                    .HasForeignKey(n => n.PuestoKey)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.Departamento)
                    .WithMany()
                    .HasForeignKey(n => n.DepartamentoKey)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuraci�n de Asistencia
            modelBuilder.Entity<Asistencia>(entity =>
            {
                entity.ToTable("Asistencia", "FACT");
                entity.HasKey(a => a.AsistenciaKey);

                entity.HasOne(a => a.Empleado)
                    .WithMany()
                    .HasForeignKey(a => a.EmpleadoKey)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(a => new { a.EmpleadoKey, a.FechaKey })
                    .IsUnique();
            });

            // Configuraci�n de Vacaciones
            modelBuilder.Entity<Vacaciones>(entity =>
            {
                entity.ToTable("Vacaciones", "FACT");
                entity.HasKey(v => v.VacacionKey);

                entity.HasOne(v => v.Empleado)
                    .WithMany()
                    .HasForeignKey(v => v.EmpleadoKey)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(v => v.Aprobador)
                    .WithMany()
                    .HasForeignKey(v => v.AprobadorKey)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuraci�n de Rendimiento
            modelBuilder.Entity<Rendimiento>(entity =>
            {
                entity.ToTable("Rendimiento", "FACT");
                entity.HasKey(r => r.RendimientoKey);

                entity.HasOne(r => r.Empleado)
                    .WithMany()
                    .HasForeignKey(r => r.EmpleadoKey)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Evaluador)
                    .WithMany()
                    .HasForeignKey(r => r.EvaluadorKey)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}