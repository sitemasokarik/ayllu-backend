using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcodePe.Catering.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Evento__UsuarioI__5DCAEF64",
                table: "Evento");

            migrationBuilder.DropIndex(
                name: "IX_Evento_UsuarioID",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "UsuarioID",
                table: "Evento");

            migrationBuilder.AddColumn<int>(
                name: "EventoID",
                table: "Cotizacion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cotizacion_EventoID",
                table: "Cotizacion",
                column: "EventoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cotizacion_Evento",
                table: "Cotizacion",
                column: "EventoID",
                principalTable: "Evento",
                principalColumn: "EventoID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cotizacion_Evento",
                table: "Cotizacion");

            migrationBuilder.DropIndex(
                name: "IX_Cotizacion_EventoID",
                table: "Cotizacion");

            migrationBuilder.DropColumn(
                name: "EventoID",
                table: "Cotizacion");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "Evento",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UsuarioID",
                table: "Evento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Evento_UsuarioID",
                table: "Evento",
                column: "UsuarioID");

            migrationBuilder.AddForeignKey(
                name: "FK__Evento__UsuarioI__5DCAEF64",
                table: "Evento",
                column: "UsuarioID",
                principalTable: "Usuario",
                principalColumn: "UsuarioID");
        }
    }
}
