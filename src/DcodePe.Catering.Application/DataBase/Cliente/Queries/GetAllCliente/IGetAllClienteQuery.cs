namespace DcodePe.Catering.Application.DataBase.Cliente.Queries.GetAllCliente
{
    public interface IGetAllClienteQuery
    {
        Task<List<GetAllClienteModel>> Execute();
        Task<GetAllClienteModel> GetByNumeroDocumento(string numeroDocumento);
    }
}
