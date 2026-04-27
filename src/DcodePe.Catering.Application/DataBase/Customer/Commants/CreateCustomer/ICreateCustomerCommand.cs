namespace DcodePe.Catering.Application.DataBase.Customer.Commants.CreateCustomer
{
    public interface ICreateCustomerCommand
    {
        Task<CreateCustomerModel> Execute(CreateCustomerModel model);
    }
}
