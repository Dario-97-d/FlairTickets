using FlairTickets.Web.Helpers.ControllerHelpers.Interfaces;

namespace FlairTickets.Web.Helpers.ControllerHelpers
{
    public class ControllerHelpers : IControllerHelpers
    {
        public ControllerHelpers(
            IFlightControllerHelper flightControllerHelper,
            ITicketControllerHelper ticketControllerHelper)
        {
            Flights = flightControllerHelper;
            Tickets = ticketControllerHelper;
        }

        public IFlightControllerHelper Flights { get; }
        public ITicketControllerHelper Tickets { get; }
    }
}
