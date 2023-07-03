using FlairTickets.Web.Data.Repository;

namespace FlairTickets.Web.Data.Entities
{
    public class Ticket : IEntity
    {
        public int Id { get; set; }

        public Flight Flight { get; set; }

        public int Seat { get; set; }
    }
}
