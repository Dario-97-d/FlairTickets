using System.Collections.Generic;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Models.Flight;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlairTickets.Web.Helpers.ControllerHelpers.Interfaces
{
    public interface IFlightControllerHelper
    {
        Task CreateFlightAsync(InputFlightViewModel model);
        Task<DisplayFlightViewModel> GetViewModelForDisplayAsync(int flightId);
        Task<InputFlightViewModel> GetViewModelForInputAsync(int flightId);
        Task UpdateFlightAsync(InputFlightViewModel model);
        Task DeleteFlightAsync(int id);
        Task<bool> FlightExistsAsync(int id);
        Task<IndexFlightViewModel> GetViewModelForIndexAsync(SearchFlightViewModel searchModel);
        Task<IEnumerable<SelectListItem>> GetComboAirportsIataCodeAsync();
        Task<(IEnumerable<SelectListItem>, IEnumerable<SelectListItem>)> GetComboAirportsAndAirplanesAsync();
    }
}