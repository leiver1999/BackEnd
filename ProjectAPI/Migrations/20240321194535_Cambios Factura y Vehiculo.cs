using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class CambiosFacturayVehiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstadoRtv",
                table: "Vehiculos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoLicencia",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FechaLicencia",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorreoCliente",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Facturas",
                type: "bit",
                nullable: false,
                defaultValue: false);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "EstadoRtv",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "EstadoLicencia",
                table: "users");

            migrationBuilder.DropColumn(
                name: "FechaLicencia",
                table: "users");

            migrationBuilder.DropColumn(
                name: "CorreoCliente",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Facturas");
        }
    }
}
