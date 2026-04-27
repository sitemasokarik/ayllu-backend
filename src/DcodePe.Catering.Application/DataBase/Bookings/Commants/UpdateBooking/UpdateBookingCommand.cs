using AutoMapper;
using DcodePe.Catering.Application.Database;
using DcodePe.Catering.Domain.Entities.Booking;

namespace DcodePe.Catering.Application.DataBase.Bookings.Commants.UpdateBooking
{
    public class UpdateBookingCommand: IUpdateBookingCommand
    {


        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;
        public UpdateBookingCommand(IDataBaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;

        }

        public async Task<UpdateBookingModel> Execute(UpdateBookingModel model)
        {

            var entity = _mapper.Map<BookingEntity>(model);
            _databaseService.Booking.Update(entity);
            await _databaseService.SaveAsync();
            return model;

        }
    }
}
