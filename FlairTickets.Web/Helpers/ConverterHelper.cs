using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository;
using FlairTickets.Web.Models.Entities;

namespace FlairTickets.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly IAirplaneRepository _airplaneRepository;
        private readonly IAirportRepository _airportRepository;

        public ConverterHelper(
            IAirplaneRepository airplaneRepository,
            IAirportRepository airportRepository)
        {
            _airplaneRepository = airplaneRepository;
            _airportRepository = airportRepository;
        }


        public FlightViewModel FlightToViewModel(Flight flight)
        {
            return new FlightViewModel
            {
                Number = flight.Number,
                DateTime = flight.DateTime,
                OriginAirportId = flight.Origin?.Id ?? 0,
                DestinationAirportId = flight.Destination?.Id ?? 0,
                AirplaneId = flight.Airplane?.Id ?? 0,
            };
        }

        public async Task<Flight> ViewModelToFlightAsync(FlightViewModel model)
        {
            return new Flight
            {
                Id = model.Id,
                Number = model.Number,
                DateTime = model.DateTime,
                Origin = await _airportRepository.GetByIdAsync(model.OriginAirportId),
                Destination = await _airportRepository.GetByIdAsync(model.DestinationAirportId),
                Airplane = await _airplaneRepository.GetByIdAsync(model.AirplaneId)
            };
        }
    }
}
