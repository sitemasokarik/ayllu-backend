using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcodePe.Catering.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdaptBlogToLandingPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Blog__AutorID__6754599E",
                table: "Blog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Blog",
                table: "Blog");

            migrationBuilder.DropIndex(
                name: "IX_Blog_AutorID",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "AutorID",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "Contenido",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "FechaPublicación",
                table: "Blog");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioModificacion",
                table: "Blog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioEliminacion",
                table: "Blog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioCreacion",
                table: "Blog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Blog",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Blog",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Imagenes",
                table: "Blog",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioEntityUsuarioID",
                table: "Blog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValoresJson",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Blog",
                table: "Blog",
                column: "BlogID");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_Estado",
                table: "Blog",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_Titulo",
                table: "Blog",
                column: "Titulo");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_UsuarioEntityUsuarioID",
                table: "Blog",
                column: "UsuarioEntityUsuarioID");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Usuario_UsuarioEntityUsuarioID",
                table: "Blog",
                column: "UsuarioEntityUsuarioID",
                principalTable: "Usuario",
                principalColumn: "UsuarioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Usuario_UsuarioEntityUsuarioID",
                table: "Blog");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Blog",
                table: "Blog");

            migrationBuilder.DropIndex(
                name: "IX_Blog_Estado",
                table: "Blog");

            migrationBuilder.DropIndex(
                name: "IX_Blog_Titulo",
                table: "Blog");

            migrationBuilder.DropIndex(
                name: "IX_Blog_UsuarioEntityUsuarioID",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "Imagenes",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "UsuarioEntityUsuarioID",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "ValoresJson",
                table: "Blog");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioModificacion",
                table: "Blog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioEliminacion",
                table: "Blog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioCreacion",
                table: "Blog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Blog",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "AutorID",
                table: "Blog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Contenido",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPublicación",
                table: "Blog",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Blog",
                table: "Blog",
                column: "BlogID");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_AutorID",
                table: "Blog",
                column: "AutorID");

            migrationBuilder.AddForeignKey(
                name: "FK__Blog__AutorID__6754599E",
                table: "Blog",
                column: "AutorID",
                principalTable: "Usuario",
                principalColumn: "UsuarioID");
        }
    }
}
