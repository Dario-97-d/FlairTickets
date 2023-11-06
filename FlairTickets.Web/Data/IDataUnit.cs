using FlairTickets.Web.Data.Repository.Interfaces;

namespace FlairTickets.Web.Data
{
    public interface IDataUnit
    {
        IAirplaneRepository Airplanes { get; }
        IAirportRepository Airports { get; }
        IFlightRepository Flights { get; }
        ITicketRepository Tickets { get; }
    }
}