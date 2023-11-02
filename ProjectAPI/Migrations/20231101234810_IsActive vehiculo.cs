using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class IsActivevehiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehiculos",
                table: "Vehiculos");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Vehiculos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Vehiculos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehiculos",
                table: "Vehiculos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehiculos",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Vehiculos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehiculos",
                table: "Vehiculos",
                column: "IdVehiculo");
        }
    }
}
