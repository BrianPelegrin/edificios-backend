using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoEdificios.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitColorsAndBlueprintTransform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BlueprintDepth",
                table: "ProjectLayouts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BlueprintOpacity",
                table: "ProjectLayouts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BlueprintRotationY",
                table: "ProjectLayouts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BlueprintWidth",
                table: "ProjectLayouts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BlueprintX",
                table: "ProjectLayouts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BlueprintZ",
                table: "ProjectLayouts",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UnitColorSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EstadoKey = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ColorCss = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitColorSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitColorSettings_EstadoKey",
                table: "UnitColorSettings",
                column: "EstadoKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitColorSettings");

            migrationBuilder.DropColumn(
                name: "BlueprintDepth",
                table: "ProjectLayouts");

            migrationBuilder.DropColumn(
                name: "BlueprintOpacity",
                table: "ProjectLayouts");

            migrationBuilder.DropColumn(
                name: "BlueprintRotationY",
                table: "ProjectLayouts");

            migrationBuilder.DropColumn(
                name: "BlueprintWidth",
                table: "ProjectLayouts");

            migrationBuilder.DropColumn(
                name: "BlueprintX",
                table: "ProjectLayouts");

            migrationBuilder.DropColumn(
                name: "BlueprintZ",
                table: "ProjectLayouts");

        }
    }
}
