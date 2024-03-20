using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioconRequisicion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Requisiciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requisiciones_UserId",
                table: "Requisiciones",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requisiciones_users_UserId",
                table: "Requisiciones",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requisiciones_users_UserId",
                table: "Requisiciones");

            migrationBuilder.DropIndex(
                name: "IX_Requisiciones_UserId",
                table: "Requisiciones");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Requisiciones");
        }
    }
}
