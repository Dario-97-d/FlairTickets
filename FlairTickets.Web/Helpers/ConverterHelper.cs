using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers.Interfaces;
using FlairTickets.Web.Models.Flight;
using FlairTickets.Web.Models.Ticket;

namespace FlairTickets.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public DisplayFlightViewModel FlightToDisplayFlightViewModel(Flight flight)
        {
            return new DisplayFlightViewModel
            {
                Id = flight.Id,
                Number = flight.Number,
                DateTime = string.Format("{0:d} {0:HH:mm}", flight.DateTime),
            };
        }
        
        public InputFlightViewModel FlightToInputFlightViewModel(Flight flight)
        {
            return new InputFlightViewModel
            {
                Id = flight.Id,
                Number = flight.Number,
                DateTime = flight.DateTime,
                AirplaneId = flight.AirplaneId,
            };
        }

        public FlightViewModel FlightToViewModel(Flight flight)
        {
            return new FlightViewModel
            {
                Number = flight.Number,
                DateTime = flight.DateTime,
                OriginAirportId = flight.OriginAirportId,
                DestinationAirportId = flight.DestinationAirportId,
                AirplaneId = flight.AirplaneId,
            };
        }

        public DisplayTicketViewModel TicketToDisplayTicketViewModel(Ticket ticket)
        {
            return new DisplayTicketViewModel
            {
                Id = ticket.Id,
                FlightId = ticket.FlightId,
                Seat = ticket.Seat,
            };
        }

        public Flight ViewModelFlightInputToFlight(InputFlightViewModel model)
        {
            return new Flight
            {
                Id = model.Id,
                Number = model.Number,
                DateTime = model.DateTime,
                AirplaneId = model.AirplaneId,
            };
        }

        public Flight ViewModelFlightInputToFlight(InputFlightViewModel model, Flight flight)
        {
            flight.Id = model.Id;
            flight.Number = model.Number;
            flight.DateTime = model.DateTime;
            flight.AirplaneId = model.AirplaneId;

            return flight;
        }

        public Ticket ViewModelTicketInputToTicket(InputTicketViewModel model)
        {
            return new Ticket
            {
                Id = model.Id,
                Seat = model.Seat,
            };
        }

        public Ticket ViewModelTicketInputToTicket
            (InputTicketViewModel model, string userId, int flightId)
        {
            return new Ticket
            {
                Id = model.Id,
                UserId = userId,
                FlightId = flightId,
                Seat = model.Seat,
            };
        }

        public Flight ViewModelToFlight(FlightViewModel model)
        {
            return new Flight
            {
                Id = model.Id,
                Number = model.Number,
                DateTime = model.DateTime,
                OriginAirportId = model.OriginAirportId,
                DestinationAirportId = model.DestinationAirportId,
                AirplaneId = model.AirplaneId,
            };
        }
    }
}
