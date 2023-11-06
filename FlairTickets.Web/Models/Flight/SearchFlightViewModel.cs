namespace FlairTickets.Web.Models.Flight
{
    public class SearchFlightViewModel
    {
#nullable enable
        public string? FlightNumber { get; set; }

        public string? Airline { get; set; }


        public string? DateMin { get; set; }

        public string? DateMax { get; set; }


        public string? TimeMin { get; set; }

        public string? TimeMax { get; set; }


        public string? Origin { get; set; }

        public string? Destination { get; set; }


        public bool IsSearchNull =>
            FlightNumber == null && Airline == null
            && DateMin == null && DateMax == null
            && TimeMin == null && TimeMax == null
            && Origin == null && Destination == null;
    }
}
