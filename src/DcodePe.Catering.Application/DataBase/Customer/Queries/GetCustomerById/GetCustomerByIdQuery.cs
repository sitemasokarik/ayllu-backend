namespace DcodePe.Catering.Application.DataBase.Customer.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery: IGetCustomerByIdQuery
    {
        private readonly IDataBaseService _dataBaseService;
        private readonly IMapper _mapper;

        public GetCustomerByIdQuery(IDataBaseService dataBaseService, IMapper mapper)
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;


        }

        public async Task<GetCustomerByIdModel> Execute(int CustomerId)
        {
            var entity = await _dataBaseService.Customer
                .FirstOrDefaultAsync(x => x.CustomerId == CustomerId);
            return _mapper.Map<GetCustomerByIdModel>(entity);
        }
    }
}
