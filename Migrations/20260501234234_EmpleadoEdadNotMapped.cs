using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.SantaLucia.Web.Migrations
{
    /// <inheritdoc />
    public partial class EmpleadoEdadNotMapped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE s.name = N'DIM' AND t.name = N'Empleado' AND c.name = N'Edad')
    ALTER TABLE [DIM].[Empleado] DROP COLUMN [Edad];
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Edad",
                schema: "DIM",
                table: "Empleado",
                type: "int",
                nullable: true,
                computedColumnSql: "(DATEDIFF(YEAR, [FechaNacimiento], GETDATE()) - CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, [FechaNacimiento], GETDATE()), [FechaNacimiento]) > GETDATE() THEN 1 ELSE 0 END)",
                stored: true);
        }
    }
}
