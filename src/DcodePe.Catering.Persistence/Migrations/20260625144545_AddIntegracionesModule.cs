using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcodePe.Catering.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIntegracionesModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsPortalActivo",
                table: "Cliente",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Cliente",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserNamePortal",
                table: "Cliente",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ComprobanteElectronico",
                columns: table => new
                {
                    ComprobanteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Serie = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Correlativo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NumeroCompleto = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CotizacionID = table.Column<int>(type: "int", nullable: true),
                    ClienteNombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ClienteDocumento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TipoDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClienteDireccion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ClienteTelefono = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FormaPago = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MedioPago = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OpGravadas = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    OpInafectas = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    OpExoneradas = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Igv = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Recibido = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Vuelto = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    ModoEmision = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EstadoComprobante = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SunatTicket = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SunatCdr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SunatRespuesta = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioEliminacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEliminacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteElectronico", x => x.ComprobanteID);
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteSerie",
                columns: table => new
                {
                    ComprobanteSerieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Serie = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UltimoCorrelativo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteSerie", x => x.ComprobanteSerieID);
                });

            migrationBuilder.CreateTable(
                name: "TicketInterno",
                columns: table => new
                {
                    TicketID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    EstadoTicket = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Prioridad = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreadoPorUsuarioID = table.Column<int>(type: "int", nullable: true),
                    AsignadoUsuarioID = table.Column<int>(type: "int", nullable: true),
                    CreadoPorClienteID = table.Column<int>(type: "int", nullable: true),
                    RolDestinoID = table.Column<int>(type: "int", nullable: true),
                    CotizacionID = table.Column<int>(type: "int", nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioEliminacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEliminacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInterno", x => x.TicketID);
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteDetalle",
                columns: table => new
                {
                    ComprobanteDetalleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComprobanteID = table.Column<int>(type: "int", nullable: false),
                    Item = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IdTipoIgv = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TipoIgv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Igv = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioEliminacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEliminacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteDetalle", x => x.ComprobanteDetalleID);
                    table.ForeignKey(
                        name: "FK_ComprobanteDetalle_ComprobanteElectronico_ComprobanteID",
                        column: x => x.ComprobanteID,
                        principalTable: "ComprobanteElectronico",
                        principalColumn: "ComprobanteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketMensaje",
                columns: table => new
                {
                    TicketMensajeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    ClienteID = table.Column<int>(type: "int", nullable: true),
                    AutorNombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    EsInterno = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioEliminacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEliminacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMensaje", x => x.TicketMensajeID);
                    table.ForeignKey(
                        name: "FK_TicketMensaje_TicketInterno_TicketID",
                        column: x => x.TicketID,
                        principalTable: "TicketInterno",
                        principalColumn: "TicketID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_Cliente_UserNamePortal",
                table: "Cliente",
                column: "UserNamePortal",
                unique: true,
                filter: "[UserNamePortal] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteDetalle_ComprobanteID",
                table: "ComprobanteDetalle",
                column: "ComprobanteID");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteElectronico_NumeroCompleto",
                table: "ComprobanteElectronico",
                column: "NumeroCompleto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteSerie_Serie",
                table: "ComprobanteSerie",
                column: "Serie",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketMensaje_TicketID",
                table: "TicketMensaje",
                column: "TicketID");

            migrationBuilder.InsertData(
                table: "ComprobanteSerie",
                columns: new[] { "Tipo", "Serie", "UltimoCorrelativo" },
                values: new object[,]
                {
                    { "boleta", "B001", 0 },
                    { "factura", "F001", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComprobanteDetalle");

            migrationBuilder.DropTable(
                name: "ComprobanteSerie");

            migrationBuilder.DropTable(
                name: "TicketMensaje");

            migrationBuilder.DropTable(
                name: "ComprobanteElectronico");

            migrationBuilder.DropTable(
                name: "TicketInterno");

            migrationBuilder.DropIndex(
                name: "UQ_Cliente_UserNamePortal",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "EsPortalActivo",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "UserNamePortal",
                table: "Cliente");
        }
    }
}
