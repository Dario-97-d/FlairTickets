using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Models.Flight;
using FlairTickets.Web.Models.Ticket;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data
{
    public static class QueryingSelectTickets
    {
        public static async Task<IEnumerable<IndexRowTicketViewModel>>
            SelectListIndexRowViewModelAsync(IQueryable<Ticket> iQueryableTickets)
        {
            return await iQueryableTickets.Select(ticket => new IndexRowTicketViewModel
            {
                Id = ticket.Id,
                FlightNumber = ticket.Flight.Number,
                Date = ticket.Flight.DateTime.Date.ToString("d"),
                Hour = ticket.Flight.DateTime.TimeOfDay.ToString("hh\\:mm"),
                Origin = ticket.Flight.Origin.ComboName,
                Destination = ticket.Flight.Destination.ComboName,
                Seat = ticket.Seat,
                IsPast = DateTime.Compare(ticket.Flight.DateTime, DateTime.Now.AddHours(2)) < 0,
                IsEditable = DateTime.Compare(ticket.Flight.DateTime, DateTime.Now.AddHours(12)) > 0,
                IsDeletable = DateTime.Compare(ticket.Flight.DateTime, DateTime.Now.AddDays(7)) > 0,
            }).ToListAsync();
        }

        public static async Task<DisplayFlightViewModel>
            SelectSingleDisplayViewModelAsync(IQueryable<Flight> iQueryableFlights)
        {
            return await iQueryableFlights.Select(flight => new DisplayFlightViewModel
            {
                Id = flight.Id,
                DateTime = string.Format("{0:d} {0:HH:mm}", flight.DateTime),
                Number = flight.Number,
                OriginAirport = flight.Origin.ComboName,
                DestinationAirport = flight.Destination.ComboName,
                Airplane = flight.Airplane.ComboName,
            }).SingleOrDefaultAsync();
        }

        public static async Task<DisplayTicketViewModel>
            SelectSingleDisplayViewModelAsync(IQueryable<Ticket> iQueryableTicket)
        {
            return await iQueryableTicket.Select(ticket => new DisplayTicketViewModel
            {
                Id = ticket.Id,
                UserChosenName = ticket.User.ChosenName,
                UserEmail = ticket.User.Email,
                FlightId = ticket.FlightId,
                FlightDisplay = new DisplayFlightViewModel
                {
                    Id = ticket.Id,
                    DateTime = string.Format("{0:d} {0:HH:mm}", ticket.Flight.DateTime),
                    Number = ticket.Flight.Number,
                    OriginAirport = ticket.Flight.Origin.ComboName,
                    DestinationAirport = ticket.Flight.Destination.ComboName,
                    Airplane = ticket.Flight.Airplane.ComboName,
                },
                Seat = ticket.Seat,
                IsEditable = DateTime.Compare(ticket.Flight.DateTime, DateTime.Now.AddHours(12)) > 0,
                IsDeletable = DateTime.Compare(ticket.Flight.DateTime, DateTime.Now.AddDays(7)) > 0,
            }).SingleOrDefaultAsync();
        }

        public static async Task<InputTicketViewModel>
            SelectSingleInputViewModelAsync(IQueryable<Ticket> iQueryableTicket)
        {
            return await iQueryableTicket.Select(ticket => new InputTicketViewModel
            {
                Id = ticket.Id,
                UserEmail = ticket.User.Email,
                UserChosenName = ticket.User.ChosenName,
                FlightDisplay = new DisplayFlightViewModel
                {
                    Id = ticket.Flight.Id,
                    DateTime = string.Format("{0:d} {0:HH:mm}", ticket.Flight.DateTime),
                    Number = ticket.Flight.Number,
                    OriginAirport = ticket.Flight.Origin.ComboName,
                    DestinationAirport = ticket.Flight.Destination.ComboName,
                    Airplane = ticket.Flight.Airplane.ComboName,
                },
                Seat = ticket.Seat,
                MaxSeat = ticket.Flight.Airplane.Seats,
            }).SingleOrDefaultAsync();
        }

        public static async Task<InputTicketViewModel>
            SelectSingleInputViewModelAsync
            (IQueryable<Flight> iQueryableFlight, string userEmail, string userChosenName)
        {
            return await iQueryableFlight.Select(flight => new InputTicketViewModel
            {
                Id = flight.Id,
                UserEmail = userEmail,
                UserChosenName = userChosenName,
                FlightDisplay = new DisplayFlightViewModel
                {
                    Id = flight.Id,
                    DateTime = string.Format("{0:d} {0:HH:mm}", flight.DateTime),
                    Number = flight.Number,
                    OriginAirport = flight.Origin.ComboName,
                    DestinationAirport = flight.Destination.ComboName,
                    Airplane = flight.Airplane.ComboName,
                },
                Seat = 0,
                MaxSeat = flight.Airplane.Seats,
            }).SingleOrDefaultAsync();
        }
    }
}
