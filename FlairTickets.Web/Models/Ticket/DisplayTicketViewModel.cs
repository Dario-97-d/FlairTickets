using FlairTickets.Web.Models.Flight;

namespace FlairTickets.Web.Models.Ticket
{
    public class DisplayTicketViewModel
    {
        public int Id { get; set; }

        public string UserChosenName { get; set; }

        public string UserEmail { get; set; }

        public int FlightId { get; set; }

        public DisplayFlightViewModel FlightDisplay { get; set; }

        public int Seat { get; set; }

        public bool IsEditable { get; set; }

        public bool IsDeletable { get; set; }
    }
}
