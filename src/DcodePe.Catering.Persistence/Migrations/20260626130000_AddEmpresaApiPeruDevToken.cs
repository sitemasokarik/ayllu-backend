using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcodePe.Catering.Persistence.Migrations
{
    public partial class AddEmpresaApiPeruDevToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiPeruDevToken",
                table: "Empresa",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "ApiPeruDevToken", table: "Empresa");
        }
    }
}
