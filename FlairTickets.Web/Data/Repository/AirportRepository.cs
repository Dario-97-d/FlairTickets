using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository
{
    public class AirportRepository : GenericRepository<Airport>, IAirportRepository
    {
        public AirportRepository(DataContext context) : base(context)
        {
        }
    }
}
