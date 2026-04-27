using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcodePe.Catering.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCotizacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PresupuestoPorPersona",
                table: "Cotizacion",
                newName: "TotalEvento");

            migrationBuilder.RenameColumn(
                name: "CostoTotal",
                table: "Cotizacion",
                newName: "TotalCotizacion");

            migrationBuilder.AddColumn<decimal>(
                name: "CostoDePersonal",
                table: "Cotizacion",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaContacto",
                table: "Cotizacion",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Garantia",
                table: "Cotizacion",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraContacto",
                table: "Cotizacion",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioPorCubierto",
                table: "Cotizacion",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioPorCubiertoConDescuento",
                table: "Cotizacion",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SubtotalMenu",
                table: "Cotizacion",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TarifaMenuPorInvitado",
                table: "Cotizacion",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostoDePersonal",
                table: "Cotizacion");

            migrationBuilder.DropColumn(
                name: "FechaContacto",
                table: "Cotizacion");

            migrationBuilder.DropColumn(
                name: "Garantia",
                table: "Cotizacion");

            migrationBuilder.DropColumn(
                name: "HoraContacto",
                table: "Cotizacion");

            migrationBuilder.DropColumn(
                name: "PrecioPorCubierto",
                table: "Cotizacion");

            migrationBuilder.DropColumn(
                name: "PrecioPorCubiertoConDescuento",
                table: "Cotizacion");

            migrationBuilder.DropColumn(
                name: "SubtotalMenu",
                table: "Cotizacion");

            migrationBuilder.DropColumn(
                name: "TarifaMenuPorInvitado",
                table: "Cotizacion");

            migrationBuilder.RenameColumn(
                name: "TotalEvento",
                table: "Cotizacion",
                newName: "PresupuestoPorPersona");

            migrationBuilder.RenameColumn(
                name: "TotalCotizacion",
                table: "Cotizacion",
                newName: "CostoTotal");
        }
    }
}
