using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoEdificios.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apartamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodUnidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Edificio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metraje = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Inicial = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaCompletaInicial = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InicialDolar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Pagado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Adeudado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IniciadoVaciados = table.Column<bool>(type: "bit", nullable: true),
                    FechaInicioVaciados = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnInspeccion = table.Column<bool>(type: "bit", nullable: true),
                    FechaEntregaInspeccion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Legal = table.Column<bool>(type: "bit", nullable: true),
                    ResponsableLegal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaLegal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gobierno = table.Column<bool>(type: "bit", nullable: true),
                    ResponsableGobierno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaGobierno = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Micelaneos = table.Column<bool>(type: "bit", nullable: true),
                    ResponsableMicelaneos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaMicelaneos = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Inspeccion1 = table.Column<bool>(type: "bit", nullable: true),
                    FechaInspeccion1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Inspeccion2 = table.Column<bool>(type: "bit", nullable: true),
                    FechaInspeccion2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FormaPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaFormaPago = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Saldo = table.Column<bool>(type: "bit", nullable: true),
                    Entregada = table.Column<bool>(type: "bit", nullable: true),
                    Titulo = table.Column<bool>(type: "bit", nullable: true),
                    DescargadaDGII = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    provincia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    municipio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__project__3213E83F77DE65FA", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    clave = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    salt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3213E83FE77DD8CD", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "edificios",
                columns: table => new
                {
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    projectID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    units = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unitKeys = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    totalUnits = table.Column<int>(type: "int", nullable: true),
                    columns = table.Column<int>(type: "int", nullable: true),
                    rowspercolumn = table.Column<int>(type: "int", nullable: true),
                    x = table.Column<double>(type: "float", nullable: true),
                    y = table.Column<double>(type: "float", nullable: true),
                    z = table.Column<double>(type: "float", nullable: true),
                    rotation = table.Column<int>(type: "int", nullable: true),
                    color = table.Column<int>(type: "int", nullable: true),
                    footprintScale = table.Column<int>(type: "int", nullable: true),
                    cubeScale = table.Column<int>(type: "int", nullable: true),
                    ImagenPlano = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__edificio__72E12F1AA1E2E413", x => x.name);
                    table.ForeignKey(
                        name: "FK_Edificios_Project",
                        column: x => x.projectID,
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_edificios_projectID",
                table: "edificios",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "UQ__users__AB6E6164465182F2",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apartamento");

            migrationBuilder.DropTable(
                name: "edificios");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "project");
        }
    }
}
