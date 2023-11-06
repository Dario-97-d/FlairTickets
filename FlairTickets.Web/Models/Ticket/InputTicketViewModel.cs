using FlairTickets.Web.Models.Flight;

namespace FlairTickets.Web.Models.Ticket
{
    public class InputTicketViewModel
    {
        public int Id { get; set; }

        public string UserEmail { get; set; }

        public string UserChosenName { get; set; }

        public DisplayFlightViewModel FlightDisplay { get; set; }

        public int Seat { get; set; }

        public int MaxSeat { get; set; }
    }
}
