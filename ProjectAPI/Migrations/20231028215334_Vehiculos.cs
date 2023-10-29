using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class Vehiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    IdVehiculo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroPlaca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kilometraje = table.Column<long>(type: "bigint", nullable: false),
                    TipoVehiculo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoVehiculo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.IdVehiculo);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehiculos");
        }
    }
}
