using System.Collections.Generic;

namespace FlairTickets.Web.Models.Flight
{
    public class IndexFlightViewModel
    {
        public IEnumerable<IndexRowFlightViewModel> Flights { get; set; }

        public SearchFlightViewModel SearchFlight { get; set; }
    }
}
