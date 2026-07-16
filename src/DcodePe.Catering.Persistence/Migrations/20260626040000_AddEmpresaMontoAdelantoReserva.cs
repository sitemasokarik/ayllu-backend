using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcodePe.Catering.Persistence.Migrations
{
    public partial class AddEmpresaMontoAdelantoReserva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MontoAdelantoReserva",
                table: "Empresa",
                type: "decimal(18,2)",
                nullable: true,
                defaultValue: 1000m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "MontoAdelantoReserva", table: "Empresa");
        }
    }
}
