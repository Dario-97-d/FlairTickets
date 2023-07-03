using FlairTickets.Web.Data.Repository;

namespace FlairTickets.Web.Data.Entities
{
    public class Airplane : IEntity
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Name { get; set; }

        public int Seats { get; set; }
    }
}
