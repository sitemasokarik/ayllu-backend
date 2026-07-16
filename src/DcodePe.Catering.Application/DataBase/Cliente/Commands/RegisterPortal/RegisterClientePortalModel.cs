namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.RegisterPortal

{

    public class RegisterClientePortalModel

    {

        public int ClienteID { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string NumeroDocumento { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public int TotalCotizacionesVinculadas { get; set; }

    }

}


