using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcodePe.Catering.Persistence.Migrations
{
    public partial class AddBlogMissionVision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Resumen",
                table: "Blog",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MisionTitulo",
                table: "Blog",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MisionTexto",
                table: "Blog",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisionTitulo",
                table: "Blog",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisionTexto",
                table: "Blog",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Resumen", table: "Blog");
            migrationBuilder.DropColumn(name: "MisionTitulo", table: "Blog");
            migrationBuilder.DropColumn(name: "MisionTexto", table: "Blog");
            migrationBuilder.DropColumn(name: "VisionTitulo", table: "Blog");
            migrationBuilder.DropColumn(name: "VisionTexto", table: "Blog");
        }
    }
}
