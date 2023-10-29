using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class CambiosFacturas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Codigo",
                table: "Facturas",
                newName: "codigo");

            migrationBuilder.AddColumn<int>(
                name: "codigoFactura",
                table: "Facturas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codigoFactura",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "codigo",
                table: "Facturas",
                newName: "Codigo");
        }
    }
}
