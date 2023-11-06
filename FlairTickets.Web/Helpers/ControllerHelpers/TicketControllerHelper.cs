using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlairTickets.Web.Data;
using FlairTickets.Web.Helpers.ControllerHelpers.Interfaces;
using FlairTickets.Web.Helpers.Interfaces;
using FlairTickets.Web.Models.Ticket;

namespace FlairTickets.Web.Helpers.ControllerHelpers
{
    public class TicketControllerHelper : ITicketControllerHelper
    {
        private readonly IDataUnit _dataUnit;
        private readonly IHelpers _helpers;

        public TicketControllerHelper(IDataUnit dataUnit, IHelpers helpers)
        {
            _dataUnit = dataUnit;
            _helpers = helpers;
        }


        public async Task<string> CheckTicketAsync(InputTicketViewModel model, string action)
        {
            if (model.Seat < 1)
            {
                return "This seat is not valid.";
            }

            if (!await _dataUnit.Flights.IsSeatInBoundsAsync(model.FlightDisplay.Id, model.Seat))
            {
                return "This Seat is not available in this Flight.";
            }

            var ticketBuyDateTimeLimit = DateTime.Now.AddHours(2);
            if (await _dataUnit.Flights
                .IsFlightAfterDateTimeAsync(model.FlightDisplay.Id, ticketBuyDateTimeLimit))
            {
                return $"It's too late to {action} a ticket for this Flight.";
            }

            // Success
            return string.Empty;
        }

        public async Task<int> CreateTicketAsync(InputTicketViewModel model)
        {
            var user = await _helpers.User.GetUserByEmailAsync(model.UserEmail);

            var ticket = _helpers.Converter
                .ViewModelTicketInputToTicket(model, user.Id, model.FlightDisplay.Id);

            ticket = await _dataUnit.Tickets.CreateAsync(ticket);

            return ticket.Id;
        }

        public async Task DeleteTicketAsync(int id)
        {
            await _dataUnit.Tickets.DeleteAsync(id);
        }

        public async Task<IEnumerable<IndexRowTicketViewModel>> GetTicketsForIndexAsync()
        {
            var query = _dataUnit.Tickets.GetAll()
                .OrderByDescending(t => t.Flight.DateTime)
                .Take(25);

            return await QueryingSelectTickets.SelectListIndexRowViewModelAsync(query);
        }

        public async Task<IEnumerable<IndexRowTicketViewModel>> GetTicketsForIndexAsync(string userName)
        {
            var query = _dataUnit.Tickets.GetAll()
                .Where(t => t.User.UserName == userName)
                .OrderByDescending(t => t.Flight.DateTime)
                .Take(25);

            return await QueryingSelectTickets.SelectListIndexRowViewModelAsync(query);
        }

        public async Task<DisplayTicketViewModel> GetViewModelForDisplayAsync(int ticketId)
        {
            var query = _dataUnit.Tickets.WhereIdEquals(ticketId);

            return await QueryingSelectTickets.SelectSingleDisplayViewModelAsync(query);
        }

        public async Task<DisplayTicketViewModel> GetViewModelForDisplayAsync
            (int ticketId, string userName)
        {
            var query = _dataUnit.Tickets.WhereIdEquals(ticketId)
                .Where(t => t.User.UserName == userName);

            return await QueryingSelectTickets.SelectSingleDisplayViewModelAsync(query);
        }

        public async Task<InputTicketViewModel> GetViewModelForInputCreateAsync
            (int flightId, string userName)
        {
            var queryFlight = _dataUnit.Flights.WhereIdEquals(flightId);

            var userChosenName = await _helpers.User.GetUserChosenNameAsync(userName);

            return await QueryingSelectTickets
                .SelectSingleInputViewModelAsync(queryFlight, userName, userChosenName);
        }

        public async Task<InputTicketViewModel> GetViewModelForInputEditAsync
            (int ticketId, string userName)
        {
            var queryTicket = _dataUnit.Tickets.WhereIdEquals(ticketId)
                .Where(t => t.User.UserName == userName);

            return await QueryingSelectTickets.SelectSingleInputViewModelAsync(queryTicket);
        }

        public async Task<bool> IsSeatInBoundsAsync(int flightId, int seat)
        {
            if (seat < 1) return false;
            return await _dataUnit.Flights.IsSeatInBoundsAsync(flightId, seat);
        }

        public async Task<bool> IsSeatTakenAsync(int flightId, int seat, int ticketId)
        {
            return await _dataUnit.Tickets.IsSeatTakenAsync(flightId, seat, ticketId);
        }

        public async Task<bool> TicketExistsAsync(int id)
        {
            return await _dataUnit.Tickets.ExistsAsync(id);
        }

        public async Task<int> UpdateTicketAsync(InputTicketViewModel model)
        {
            var user = await _helpers.User.GetUserByEmailAsync(model.UserEmail);

            var ticket = _helpers.Converter
                .ViewModelTicketInputToTicket(model, user.Id, model.FlightDisplay.Id);

            ticket = await _dataUnit.Tickets.UpdateAsync(ticket);

            return ticket.Id;
        }
    }
}
