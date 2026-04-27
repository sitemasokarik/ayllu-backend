using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcodePe.Catering.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventoAddFotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fotos",
                table: "Evento",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "URL de la foto del evento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fotos",
                table: "Evento");
        }
    }
}
