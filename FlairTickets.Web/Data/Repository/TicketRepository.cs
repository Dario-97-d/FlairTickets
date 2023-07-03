using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(DataContext context) : base(context)
        {
        }
    }
}
