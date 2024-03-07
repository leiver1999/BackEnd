using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class addvehiculoauser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehiculoId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_VehiculoId",
                table: "users",
                column: "VehiculoId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_Vehiculos_VehiculoId",
                table: "users",
                column: "VehiculoId",
                principalTable: "Vehiculos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_Vehiculos_VehiculoId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_VehiculoId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "VehiculoId",
                table: "users");
        }
    }
}
