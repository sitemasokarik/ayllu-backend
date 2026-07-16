using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcodePe.Catering.Persistence.Migrations
{
    public partial class AddFechaReservadaAndSunatDesarrollo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaReservada",
                table: "Cotizacion",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaReservadaElegida",
                table: "PagoVoucher",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaReservadaElegida",
                table: "PagoMercadoPago",
                type: "datetime",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SunatModo",
                table: "Empresa",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "DESARROLLO",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "PRODUCCION");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "FechaReservada", table: "Cotizacion");
            migrationBuilder.DropColumn(name: "FechaReservadaElegida", table: "PagoVoucher");
            migrationBuilder.DropColumn(name: "FechaReservadaElegida", table: "PagoMercadoPago");
        }
    }
}
