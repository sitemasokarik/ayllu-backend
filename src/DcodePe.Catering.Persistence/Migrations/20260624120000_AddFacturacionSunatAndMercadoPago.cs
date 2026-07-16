using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DcodePe.Catering.Persistence.Migrations
{
    public partial class AddFacturacionSunatAndMercadoPago : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GeneraFactElect",
                table: "Empresa",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Ubigeo",
                table: "Empresa",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RutaCertificadoServidor",
                table: "Empresa",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificadoFileName",
                table: "Empresa",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaveCertificado",
                table: "Empresa",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioSol",
                table: "Empresa",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaveSol",
                table: "Empresa",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SunatModo",
                table: "Empresa",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "PRODUCCION");

            migrationBuilder.AddColumn<string>(
                name: "SunatWsProduccion",
                table: "Empresa",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PagoVoucherID",
                table: "ComprobanteElectronico",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PagoMercadoPagoID",
                table: "ComprobanteElectronico",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoAdelantoFacturado",
                table: "ComprobanteElectronico",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SunatHashCpe",
                table: "ComprobanteElectronico",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RutaXml",
                table: "ComprobanteElectronico",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RutaCdr",
                table: "ComprobanteElectronico",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SunatCodigoError",
                table: "ComprobanteElectronico",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PagoVoucher",
                columns: table => new
                {
                    PagoVoucherID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CotizacionID = table.Column<int>(type: "int", nullable: false),
                    ClienteID = table.Column<int>(type: "int", nullable: false),
                    ArchivoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    EstadoPago = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ObservacionCliente = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ObservacionAdmin = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagoVoucher", x => x.PagoVoucherID);
                    table.ForeignKey(
                        name: "FK_PagoVoucher_Cotizacion_CotizacionID",
                        column: x => x.CotizacionID,
                        principalTable: "Cotizacion",
                        principalColumn: "CotizacionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagoVoucher_Cliente_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Cliente",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PagoMercadoPago",
                columns: table => new
                {
                    PagoMercadoPagoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CotizacionID = table.Column<int>(type: "int", nullable: false),
                    ClienteID = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    MpPaymentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MpPreferenceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EstadoPago = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MpStatusDetail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaPago = table.Column<DateTime>(type: "datetime", nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagoMercadoPago", x => x.PagoMercadoPagoID);
                    table.ForeignKey(
                        name: "FK_PagoMercadoPago_Cotizacion_CotizacionID",
                        column: x => x.CotizacionID,
                        principalTable: "Cotizacion",
                        principalColumn: "CotizacionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagoMercadoPago_Cliente_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Cliente",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(name: "IX_PagoVoucher_EstadoPago", table: "PagoVoucher", column: "EstadoPago");
            migrationBuilder.CreateIndex(name: "IX_PagoVoucher_CotizacionID", table: "PagoVoucher", column: "CotizacionID");
            migrationBuilder.CreateIndex(name: "UQ_PagoMercadoPago_MpPaymentId", table: "PagoMercadoPago", column: "MpPaymentId", unique: true);
            migrationBuilder.CreateIndex(name: "IX_PagoMercadoPago_CotizacionID", table: "PagoMercadoPago", column: "CotizacionID");
            migrationBuilder.CreateIndex(name: "IX_PagoMercadoPago_EstadoPago", table: "PagoMercadoPago", column: "EstadoPago");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PagoMercadoPago");
            migrationBuilder.DropTable(name: "PagoVoucher");

            migrationBuilder.DropColumn(name: "GeneraFactElect", table: "Empresa");
            migrationBuilder.DropColumn(name: "Ubigeo", table: "Empresa");
            migrationBuilder.DropColumn(name: "RutaCertificadoServidor", table: "Empresa");
            migrationBuilder.DropColumn(name: "CertificadoFileName", table: "Empresa");
            migrationBuilder.DropColumn(name: "ClaveCertificado", table: "Empresa");
            migrationBuilder.DropColumn(name: "UsuarioSol", table: "Empresa");
            migrationBuilder.DropColumn(name: "ClaveSol", table: "Empresa");
            migrationBuilder.DropColumn(name: "SunatModo", table: "Empresa");
            migrationBuilder.DropColumn(name: "SunatWsProduccion", table: "Empresa");
            migrationBuilder.DropColumn(name: "PagoVoucherID", table: "ComprobanteElectronico");
            migrationBuilder.DropColumn(name: "PagoMercadoPagoID", table: "ComprobanteElectronico");
            migrationBuilder.DropColumn(name: "MontoAdelantoFacturado", table: "ComprobanteElectronico");
            migrationBuilder.DropColumn(name: "SunatHashCpe", table: "ComprobanteElectronico");
            migrationBuilder.DropColumn(name: "RutaXml", table: "ComprobanteElectronico");
            migrationBuilder.DropColumn(name: "RutaCdr", table: "ComprobanteElectronico");
            migrationBuilder.DropColumn(name: "SunatCodigoError", table: "ComprobanteElectronico");
        }
    }
}
