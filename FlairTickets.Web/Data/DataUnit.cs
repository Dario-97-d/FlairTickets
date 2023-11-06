using FlairTickets.Web.Data.Repository.Interfaces;

namespace FlairTickets.Web.Data
{
    public class DataUnit : IDataUnit
    {
        public DataUnit(
            IAirplaneRepository airplaneRepository,
            IAirportRepository airportRepository,
            IFlightRepository flightRepository,
            ITicketRepository ticketRepository)
        {
            Airplanes = airplaneRepository;
            Airports = airportRepository;
            Flights = flightRepository;
            Tickets = ticketRepository;
        }

        public IAirplaneRepository Airplanes { get; }
        public IAirportRepository Airports { get; }
        public IFlightRepository Flights { get; }
        public ITicketRepository Tickets { get; }
    }
}
