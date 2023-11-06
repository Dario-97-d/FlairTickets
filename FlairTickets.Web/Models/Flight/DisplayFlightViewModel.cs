namespace FlairTickets.Web.Models.Flight
{
    public class DisplayFlightViewModel
    {
        public int Id { get; set; }

        public string DateTime { get; set; }

        public string Number { get; set; }

        public string OriginAirport { get; set; }

        public string DestinationAirport { get; set; }

        public string Airplane { get; set; }
    }
}
