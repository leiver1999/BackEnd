using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class Requisicionyfacturas2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Requisicion_RequisicionId",
                table: "Facturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requisicion",
                table: "Requisicion");

            migrationBuilder.RenameTable(
                name: "Requisicion",
                newName: "Requisiciones");

            migrationBuilder.AlterColumn<int>(
                name: "RequisicionId",
                table: "Facturas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requisiciones",
                table: "Requisiciones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Requisiciones_RequisicionId",
                table: "Facturas",
                column: "RequisicionId",
                principalTable: "Requisiciones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Requisiciones_RequisicionId",
                table: "Facturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requisiciones",
                table: "Requisiciones");

            migrationBuilder.RenameTable(
                name: "Requisiciones",
                newName: "Requisicion");

            migrationBuilder.AlterColumn<int>(
                name: "RequisicionId",
                table: "Facturas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requisicion",
                table: "Requisicion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Requisicion_RequisicionId",
                table: "Facturas",
                column: "RequisicionId",
                principalTable: "Requisicion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
