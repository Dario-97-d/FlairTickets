using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository
{
    public class AirplaneRepository : GenericRepository<Airplane>, IAirplaneRepository
    {
        public AirplaneRepository(DataContext context) : base(context)
        {
        }
    }
}
