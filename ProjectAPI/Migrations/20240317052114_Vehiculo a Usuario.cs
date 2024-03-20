using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class VehiculoaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_VehiculoId",
                table: "users");

            migrationBuilder.CreateIndex(
                name: "IX_users_VehiculoId",
                table: "users",
                column: "VehiculoId",
                unique: true,
                filter: "[VehiculoId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_VehiculoId",
                table: "users");

            migrationBuilder.CreateIndex(
                name: "IX_users_VehiculoId",
                table: "users",
                column: "VehiculoId");
        }
    }
}
