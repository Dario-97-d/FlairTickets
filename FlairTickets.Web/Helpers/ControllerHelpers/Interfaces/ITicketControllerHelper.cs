using System.Collections.Generic;
using System.Threading.Tasks;
using FlairTickets.Web.Models.Ticket;

namespace FlairTickets.Web.Helpers.ControllerHelpers.Interfaces
{
    public interface ITicketControllerHelper
    {
        Task<string> CheckTicketAsync(InputTicketViewModel model, string action);
        Task<int> CreateTicketAsync(InputTicketViewModel model);
        Task DeleteTicketAsync(int id);
        Task<IEnumerable<IndexRowTicketViewModel>> GetTicketsBookedAsync(string userName);
        Task<IEnumerable<IndexRowTicketViewModel>> GetTicketsForIndexAsync();
        Task<IEnumerable<IndexRowTicketViewModel>> GetTicketsForIndexAsync(string userName);
        Task<IEnumerable<IndexRowTicketViewModel>> GetTicketsHistoryAsync(string userName);
        Task<DisplayTicketViewModel> GetViewModelForDisplayAsync(int ticketId);
        Task<DisplayTicketViewModel> GetViewModelForDisplayAsync(int ticketId, string userName);
        Task<InputTicketViewModel> GetViewModelForInputCreateAsync(int flightId, string userName);
        Task<InputTicketViewModel> GetViewModelForInputEditAsync(int ticketId, string userName);
        Task<bool> IsSeatInBoundsAsync(int flightId, int seat);
        Task<bool> IsSeatTakenAsync(int flightId, int seat, int ticketId);
        Task<bool> TicketExistsAsync(int id);
        Task<int> UpdateTicketAsync(InputTicketViewModel model);
    }
}