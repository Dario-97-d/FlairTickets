namespace FlairTickets.Web.Helpers.ControllerHelpers.Interfaces
{
    public interface IControllerHelpers
    {
        IFlightControllerHelper Flights { get; }
        ITicketControllerHelper Tickets { get; }
    }
}