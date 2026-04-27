using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcodePe.Catering.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoriaRecursiva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaPadreID",
                table: "Categoria",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icono",
                table: "Categoria",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Nivel",
                table: "Categoria",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "Categoria",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_CategoriaPadreID",
                table: "Categoria",
                column: "CategoriaPadreID");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Nivel",
                table: "Categoria",
                column: "Nivel");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Padre_Orden",
                table: "Categoria",
                columns: new[] { "CategoriaPadreID", "Orden" });

            migrationBuilder.AddForeignKey(
                name: "FK_Categoria_CategoriaPadre",
                table: "Categoria",
                column: "CategoriaPadreID",
                principalTable: "Categoria",
                principalColumn: "CategoriaID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categoria_CategoriaPadre",
                table: "Categoria");

            migrationBuilder.DropIndex(
                name: "IX_Categoria_CategoriaPadreID",
                table: "Categoria");

            migrationBuilder.DropIndex(
                name: "IX_Categoria_Nivel",
                table: "Categoria");

            migrationBuilder.DropIndex(
                name: "IX_Categoria_Padre_Orden",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "CategoriaPadreID",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "Icono",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "Orden",
                table: "Categoria");
        }
    }
}
