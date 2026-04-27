namespace DcodePe.Catering.Domain.Entities.Customer
{
    public class CustomerEntity
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string DocumentNumber { get; set; }

        public ICollection<BookingEntity>  Bookings { get; set; }
    }
}
