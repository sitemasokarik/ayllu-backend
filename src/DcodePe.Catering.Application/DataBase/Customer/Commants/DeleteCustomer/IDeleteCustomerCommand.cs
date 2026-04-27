namespace DcodePe.Catering.Application.DataBase.Customer.Commants.DeleteCustomer
{
    public interface IDeleteCustomerCommand
    {
        Task<bool> Execute(int CustomerId);
    }
}
