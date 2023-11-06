using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository.Interfaces;

namespace FlairTickets.Web.Data.Repository
{
    public class AirplaneRepository : GenericRepository<Airplane>, IAirplaneRepository
    {
        public AirplaneRepository(DataContext context) : base(context)
        {
        }
    }
}
