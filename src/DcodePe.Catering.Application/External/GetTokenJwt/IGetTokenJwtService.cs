namespace DcodePe.Catering.Application.External.GetTokenJwt
{
    public interface IGetTokenJwtService
    {
        string Execute(string id, string userType = "admin");
    }
}
