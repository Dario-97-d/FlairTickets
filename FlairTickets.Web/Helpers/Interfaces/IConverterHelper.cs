using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Models.Flight;
using FlairTickets.Web.Models.Ticket;

namespace FlairTickets.Web.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        DisplayFlightViewModel FlightToDisplayFlightViewModel(Flight flight);
        InputFlightViewModel FlightToInputFlightViewModel(Flight flight);
        FlightViewModel FlightToViewModel(Flight flight);
        DisplayTicketViewModel TicketToDisplayTicketViewModel(Ticket ticket);
        Flight ViewModelFlightInputToFlight(InputFlightViewModel model, Flight flight);
        Flight ViewModelFlightInputToFlight(InputFlightViewModel model);
        Ticket ViewModelTicketInputToTicket(InputTicketViewModel model);
        Ticket ViewModelTicketInputToTicket(InputTicketViewModel model, string userId, int flightId);
        Flight ViewModelToFlight(FlightViewModel model);
    }
}