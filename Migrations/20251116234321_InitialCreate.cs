using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.SantaLucia.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "FACT");

            migrationBuilder.EnsureSchema(
                name: "DIM");

            migrationBuilder.CreateTable(
                name: "Banco",
                schema: "DIM",
                columns: table => new
                {
                    BancoKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreBanco = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodigoBanco = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banco", x => x.BancoKey);
                });

            migrationBuilder.CreateTable(
                name: "Puesto",
                schema: "DIM",
                columns: table => new
                {
                    PuestoKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePuesto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SalarioMinimo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalarioMaximo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TipoPuesto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puesto", x => x.PuestoKey);
                });

            migrationBuilder.CreateTable(
                name: "Asistencia",
                schema: "FACT",
                columns: table => new
                {
                    AsistenciaKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoKey = table.Column<int>(type: "int", nullable: false),
                    FechaKey = table.Column<int>(type: "int", nullable: false),
                    HoraEntrada = table.Column<TimeSpan>(type: "time", nullable: true),
                    HoraSalida = table.Column<TimeSpan>(type: "time", nullable: true),
                    HorasTrabajadas = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MinutosTarde = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Justificacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencia", x => x.AsistenciaKey);
                });

            migrationBuilder.CreateTable(
                name: "Departamento",
                schema: "DIM",
                columns: table => new
                {
                    DepartamentoKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreDepartamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DepartamentoPadreKey = table.Column<int>(type: "int", nullable: true),
                    JefeKey = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.DepartamentoKey);
                    table.ForeignKey(
                        name: "FK_Departamento_Departamento_DepartamentoPadreKey",
                        column: x => x.DepartamentoPadreKey,
                        principalSchema: "DIM",
                        principalTable: "Departamento",
                        principalColumn: "DepartamentoKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Empleado",
                schema: "DIM",
                columns: table => new
                {
                    EmpleadoKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NombreCompleto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, computedColumnSql: "[Nombre] + ' ' + [Apellido1] + ' ' + ISNULL([Apellido2], '')", stored: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailPersonal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TelefonoEmergencia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContactoEmergencia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(DATEDIFF(YEAR, [FechaNacimiento], GETDATE()) - CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, [FechaNacimiento], GETDATE()), [FechaNacimiento]) > GETDATE() THEN 1 ELSE 0 END)", stored: true),
                    Genero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EstadoCivil = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Nacionalidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Canton = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Distrito = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DireccionExacta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaSalida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TipoContrato = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PuestoKey = table.Column<int>(type: "int", nullable: true),
                    DepartamentoKey = table.Column<int>(type: "int", nullable: true),
                    CuentaBancaria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BancoKey = table.Column<int>(type: "int", nullable: true),
                    NivelEducativo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleado", x => x.EmpleadoKey);
                    table.ForeignKey(
                        name: "FK_Empleado_Banco_BancoKey",
                        column: x => x.BancoKey,
                        principalSchema: "DIM",
                        principalTable: "Banco",
                        principalColumn: "BancoKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Empleado_Departamento_DepartamentoKey",
                        column: x => x.DepartamentoKey,
                        principalSchema: "DIM",
                        principalTable: "Departamento",
                        principalColumn: "DepartamentoKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Empleado_Puesto_PuestoKey",
                        column: x => x.PuestoKey,
                        principalSchema: "DIM",
                        principalTable: "Puesto",
                        principalColumn: "PuestoKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Nomina",
                schema: "FACT",
                columns: table => new
                {
                    NominaKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoKey = table.Column<int>(type: "int", nullable: false),
                    PuestoKey = table.Column<int>(type: "int", nullable: false),
                    DepartamentoKey = table.Column<int>(type: "int", nullable: false),
                    PeriodoKey = table.Column<int>(type: "int", nullable: false),
                    SalarioBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorasTrabajadas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorasExtra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PagoHorasExtra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bonificaciones = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comisiones = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalExtras = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SeguroSocial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Renta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtrasDeducciones = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDeducciones = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalarioNeto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nomina", x => x.NominaKey);
                    table.ForeignKey(
                        name: "FK_Nomina_Departamento_DepartamentoKey",
                        column: x => x.DepartamentoKey,
                        principalSchema: "DIM",
                        principalTable: "Departamento",
                        principalColumn: "DepartamentoKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nomina_Empleado_EmpleadoKey",
                        column: x => x.EmpleadoKey,
                        principalSchema: "DIM",
                        principalTable: "Empleado",
                        principalColumn: "EmpleadoKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nomina_Puesto_PuestoKey",
                        column: x => x.PuestoKey,
                        principalSchema: "DIM",
                        principalTable: "Puesto",
                        principalColumn: "PuestoKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rendimiento",
                schema: "FACT",
                columns: table => new
                {
                    RendimientoKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoKey = table.Column<int>(type: "int", nullable: false),
                    EvaluadorKey = table.Column<int>(type: "int", nullable: false),
                    FechaKey = table.Column<int>(type: "int", nullable: false),
                    CalificacionGeneral = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MetasPropuestas = table.Column<int>(type: "int", nullable: false),
                    MetasAlcanzadas = table.Column<int>(type: "int", nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PeriodoEvaluacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rendimiento", x => x.RendimientoKey);
                    table.ForeignKey(
                        name: "FK_Rendimiento_Empleado_EmpleadoKey",
                        column: x => x.EmpleadoKey,
                        principalSchema: "DIM",
                        principalTable: "Empleado",
                        principalColumn: "EmpleadoKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rendimiento_Empleado_EvaluadorKey",
                        column: x => x.EvaluadorKey,
                        principalSchema: "DIM",
                        principalTable: "Empleado",
                        principalColumn: "EmpleadoKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vacaciones",
                schema: "FACT",
                columns: table => new
                {
                    VacacionKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoKey = table.Column<int>(type: "int", nullable: false),
                    FechaInicioKey = table.Column<int>(type: "int", nullable: false),
                    FechaFinKey = table.Column<int>(type: "int", nullable: false),
                    DiasSolicitados = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AprobadorKey = table.Column<int>(type: "int", nullable: true),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaAprobacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacaciones", x => x.VacacionKey);
                    table.ForeignKey(
                        name: "FK_Vacaciones_Empleado_AprobadorKey",
                        column: x => x.AprobadorKey,
                        principalSchema: "DIM",
                        principalTable: "Empleado",
                        principalColumn: "EmpleadoKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vacaciones_Empleado_EmpleadoKey",
                        column: x => x.EmpleadoKey,
                        principalSchema: "DIM",
                        principalTable: "Empleado",
                        principalColumn: "EmpleadoKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asistencia_EmpleadoKey_FechaKey",
                schema: "FACT",
                table: "Asistencia",
                columns: new[] { "EmpleadoKey", "FechaKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departamento_DepartamentoPadreKey",
                schema: "DIM",
                table: "Departamento",
                column: "DepartamentoPadreKey");

            migrationBuilder.CreateIndex(
                name: "IX_Departamento_JefeKey",
                schema: "DIM",
                table: "Departamento",
                column: "JefeKey");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_BancoKey",
                schema: "DIM",
                table: "Empleado",
                column: "BancoKey");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_DepartamentoKey",
                schema: "DIM",
                table: "Empleado",
                column: "DepartamentoKey");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_PuestoKey",
                schema: "DIM",
                table: "Empleado",
                column: "PuestoKey");

            migrationBuilder.CreateIndex(
                name: "IX_Nomina_DepartamentoKey",
                schema: "FACT",
                table: "Nomina",
                column: "DepartamentoKey");

            migrationBuilder.CreateIndex(
                name: "IX_Nomina_EmpleadoKey",
                schema: "FACT",
                table: "Nomina",
                column: "EmpleadoKey");

            migrationBuilder.CreateIndex(
                name: "IX_Nomina_PuestoKey",
                schema: "FACT",
                table: "Nomina",
                column: "PuestoKey");

            migrationBuilder.CreateIndex(
                name: "IX_Rendimiento_EmpleadoKey",
                schema: "FACT",
                table: "Rendimiento",
                column: "EmpleadoKey");

            migrationBuilder.CreateIndex(
                name: "IX_Rendimiento_EvaluadorKey",
                schema: "FACT",
                table: "Rendimiento",
                column: "EvaluadorKey");

            migrationBuilder.CreateIndex(
                name: "IX_Vacaciones_AprobadorKey",
                schema: "FACT",
                table: "Vacaciones",
                column: "AprobadorKey");

            migrationBuilder.CreateIndex(
                name: "IX_Vacaciones_EmpleadoKey",
                schema: "FACT",
                table: "Vacaciones",
                column: "EmpleadoKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Asistencia_Empleado_EmpleadoKey",
                schema: "FACT",
                table: "Asistencia",
                column: "EmpleadoKey",
                principalSchema: "DIM",
                principalTable: "Empleado",
                principalColumn: "EmpleadoKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departamento_Empleado_JefeKey",
                schema: "DIM",
                table: "Departamento",
                column: "JefeKey",
                principalSchema: "DIM",
                principalTable: "Empleado",
                principalColumn: "EmpleadoKey",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departamento_Empleado_JefeKey",
                schema: "DIM",
                table: "Departamento");

            migrationBuilder.DropTable(
                name: "Asistencia",
                schema: "FACT");

            migrationBuilder.DropTable(
                name: "Nomina",
                schema: "FACT");

            migrationBuilder.DropTable(
                name: "Rendimiento",
                schema: "FACT");

            migrationBuilder.DropTable(
                name: "Vacaciones",
                schema: "FACT");

            migrationBuilder.DropTable(
                name: "Empleado",
                schema: "DIM");

            migrationBuilder.DropTable(
                name: "Banco",
                schema: "DIM");

            migrationBuilder.DropTable(
                name: "Departamento",
                schema: "DIM");

            migrationBuilder.DropTable(
                name: "Puesto",
                schema: "DIM");
        }
    }
}
