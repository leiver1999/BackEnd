using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class Requisicionyfacturas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequisicionId",
                table: "Facturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Requisicion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisicion", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_RequisicionId",
                table: "Facturas",
                column: "RequisicionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Requisicion_RequisicionId",
                table: "Facturas",
                column: "RequisicionId",
                principalTable: "Requisicion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Requisicion_RequisicionId",
                table: "Facturas");

            migrationBuilder.DropTable(
                name: "Requisicion");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_RequisicionId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "RequisicionId",
                table: "Facturas");
        }
    }
}
