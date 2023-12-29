namespace FlairTickets.Web.Models.Flight
{
    public class IndexRowFlightViewModel
    {
        public int FlightId { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string CompanyLogoUrl { get; set; }

        public string FlightNumber { get; set; }

        public string Origin { get; set; }

        public string OriginCountryCode2Letters { get; set; }

        public string Destination { get; set; }

        public string DestinationCountryCode2Letters { get; set; }
    }
}
