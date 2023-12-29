using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlairTickets.Web.Data;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers.ControllerHelpers.Interfaces;
using FlairTickets.Web.Helpers.Interfaces;
using FlairTickets.Web.Models.Flight;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Helpers.ControllerHelpers
{
    public class FlightControllerHelper : IFlightControllerHelper
    {
        private readonly IDataUnit _dataUnit;
        private readonly IHelpers _helpers;

        public FlightControllerHelper(IDataUnit dataUnit, IHelpers helpers)
        {
            _dataUnit = dataUnit;
            _helpers = helpers;
        }


        public async Task CreateFlightAsync(InputFlightViewModel model)
        {
            await _dataUnit.Flights.CreateAsync(await GetFlightFromInputViewModelAsync(model));
        }

        public async Task DeleteFlightAsync(int id)
        {
            await _dataUnit.Flights.DeleteAsync(id);
        }

        public async Task<bool> FlightExistsAsync(int id)
        {
            return await _dataUnit.Flights.ExistsAsync(id);
        }

        public async Task<(IEnumerable<SelectListItem>, IEnumerable<SelectListItem>)>
            GetComboAirportsAndAirplanesAsync()
        {
            return (

                await _dataUnit.Airports.GetAll().Select(a => new SelectListItem
                {
                    Value = a.IataCode.ToString(),
                    Text = a.ComboName,
                }).ToListAsync(),

                await _dataUnit.Airplanes.GetAll().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.ComboName,
                }).ToListAsync()

                );
        }

        public async Task<IEnumerable<SelectListItem>> GetComboAirportsIataCodeAsync()
        {
            return await _dataUnit.Airports.GetAll()
                .Select(a => new SelectListItem
                {
                    Text = a.ComboName,
                    Value = a.IataCode,
                }).ToListAsync();
        }

        public async Task<DisplayFlightViewModel> GetViewModelForDisplayAsync(int flightId)
        {
            return await _dataUnit.Flights.WhereIdEquals(flightId)
                .Select(f => new DisplayFlightViewModel
                {
                    Id = f.Id,
                    DateTime = string.Format("{0:d} {0:HH:mm}", f.DateTime),
                    Number = f.Number,
                    OriginAirport = f.Origin.ComboName,
                    OriginCountryCode2Letters = f.Origin.CountryCode2Letters,
                    DestinationAirport = f.Destination.ComboName,
                    DestinationCountryCode2Letters = f.Destination.CountryCode2Letters,
                    Airplane = f.Airplane.ComboName,
                }).SingleOrDefaultAsync();
        }

        public async Task<IndexFlightViewModel> GetViewModelForIndexAsync
            (SearchFlightViewModel searchModel)
        {
            if (searchModel == null || searchModel.IsSearchNull)
            {
                var dateTimeToday = DateTime.Today;
                searchModel = new SearchFlightViewModel
                {
                    DateMin = dateTimeToday.ToString("yyyy-MM-dd"),
                    DateMax = dateTimeToday.AddDays(1).ToString("yyyy-MM-dd"),
                };
            }

            try
            {
                return new IndexFlightViewModel
                {
                    Flights = await SearchFlightsAsync(searchModel),
                    SearchFlight = searchModel,
                };
            }
            catch { throw; }
        }

        public async Task<InputFlightViewModel> GetViewModelForInputAsync(int flightId)
        {
            return await _dataUnit.Flights.WhereIdEquals(flightId)
                .Select(f => new InputFlightViewModel
                {
                    Id = f.Id,
                    Number = f.Number,
                    DateTime = f.DateTime,
                    OriginAirportIataCode = f.Origin.IataCode,
                    DestinationAirportIataCode = f.Destination.IataCode,
                    AirplaneId = f.AirplaneId,
                }).SingleOrDefaultAsync();
        }

        public async Task UpdateFlightAsync(InputFlightViewModel model)
        {
            await _dataUnit.Flights.UpdateAsync(await GetFlightFromInputViewModelAsync(model));
        }


        private async Task<Flight> GetFlightFromInputViewModelAsync(InputFlightViewModel model)
        {
            var flight = _helpers.Converter.ViewModelFlightInputToFlight(model);

            flight.OriginAirportId = await _dataUnit.Airports
                .GetIdFromIataCodeAsync(model.OriginAirportIataCode);

            flight.DestinationAirportId = await _dataUnit.Airports
                .GetIdFromIataCodeAsync(model.DestinationAirportIataCode);

            flight.AirplaneId = model.AirplaneId;

            return flight;
        }

        private async Task<IEnumerable<IndexRowFlightViewModel>> SearchFlightsAsync(SearchFlightViewModel model)
        {
            // Get IQueryable flights.
            var flights = _dataUnit.Flights.GetAll();

            // Filter flights according to Search model.
            /* Filtering order:
             * 1- Flight number
             * 2- Date (min and max)
             * 3- Time (min and max)
             * 4- Airline
             * 5- Origin
             * 6- Destination
             */

            // Filter flights by Flight Number
            if (!string.IsNullOrEmpty(model.FlightNumber))
                flights = flights.Where(f => f.Number == model.FlightNumber);

            // Filter flights by min and max Date
            try
            {
                var isDateMinGiven = !string.IsNullOrEmpty(model.DateMin);
                var isDateMaxGiven = !string.IsNullOrEmpty(model.DateMax);

                if (isDateMinGiven && isDateMaxGiven)
                {
                    var minDate = DateTime.Parse(model.DateMin);
                    var maxDate = DateTime.Parse(model.DateMax).AddDays(1);

                    flights = flights.Where(f =>
                        DateTime.Compare(f.DateTime, minDate) >= 0
                        && DateTime.Compare(f.DateTime, maxDate) <= 0);
                }
                else if (isDateMinGiven)
                {
                    var minDate = DateTime.Parse(model.DateMin);

                    flights = flights.Where(f => DateTime.Compare(f.DateTime, minDate) >= 0);
                }
                else if (isDateMaxGiven)
                {
                    var maxDate = DateTime.Parse(model.DateMax).AddDays(1);

                    flights = flights.Where(f => DateTime.Compare(f.DateTime, maxDate) <= 0);
                }
            }
            catch
            {
                throw new FormatException("Date format error.");
            }

            // Filter flights by min and max Time
            try
            {
                var isTimeMinGiven = !string.IsNullOrEmpty(model.TimeMin);
                var isTimeMaxGIven = !string.IsNullOrEmpty(model.TimeMax);

                if (isTimeMinGiven && isTimeMaxGIven)
                {
                    var minTime = TimeSpan.Parse(model.TimeMin);
                    var maxTime = TimeSpan.Parse(model.TimeMax);

                    flights = flights.Where(f =>
                        TimeSpan.Compare(f.DateTime.TimeOfDay, minTime) >= 0
                        && TimeSpan.Compare(f.DateTime.TimeOfDay, maxTime) <= 0);
                }
                else if (isTimeMinGiven)
                {
                    var minTime = TimeSpan.Parse(model.TimeMin);

                    flights = flights.Where(f => TimeSpan.Compare(f.DateTime.TimeOfDay, minTime) >= 0);
                }
                else if (isTimeMaxGIven)
                {
                    var maxTime = TimeSpan.Parse(model.TimeMax);

                    flights = flights.Where(f => TimeSpan.Compare(f.DateTime.TimeOfDay, maxTime) <= 0);
                }
            }
            catch
            {
                throw new FormatException("Time format error.");
            }

            // TODO: Filter flights by company name

            // Filter flights by Origin
            if (!string.IsNullOrEmpty(model.Origin))
                flights = flights.Where(f =>
                    f.Origin.IataCode == model.Origin
                    || f.Origin.City.Contains(model.Origin)
                    || model.Origin.Contains(f.Origin.City));

            // Filter flights by Destination
            if (!string.IsNullOrEmpty(model.Destination))
                flights = flights.Where(f =>
                    f.Destination.IataCode == model.Destination
                    || f.Destination.City.Contains(model.Destination)
                    || model.Destination.Contains(f.Destination.City));

            // Order by DateTime then Flight Number
            flights = flights.OrderByDescending(f => f.DateTime).ThenBy(f => f.Number);

            return await flights.Take(25)
                .Select(f => new IndexRowFlightViewModel
                {
                    FlightId = f.Id,
                    Date = f.DateTime.ToShortDateString(),
                    Time = f.DateTime.ToShortTimeString(),
                    CompanyLogoUrl = "N/A",
                    FlightNumber = f.Number,
                    Origin = f.Origin.ComboName,
                    OriginCountryCode2Letters = f.Origin.CountryCode2Letters,
                    Destination = f.Destination.ComboName,
                    DestinationCountryCode2Letters = f.Origin.CountryCode2Letters,
                }).ToListAsync();
        }
    }
}
