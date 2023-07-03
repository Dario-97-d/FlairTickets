using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(DataContext context) : base(context)
        {
        }
    }
}
