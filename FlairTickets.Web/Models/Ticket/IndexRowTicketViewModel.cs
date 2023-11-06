namespace FlairTickets.Web.Models.Ticket
{
    public class IndexRowTicketViewModel
    {
        public int Id { get; set; }

        public string FlightNumber { get; set; }

        public string Date { get; set; }

        public string Hour { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public int Seat { get; set; }

        public bool IsPast { get; set; }

        public bool IsEditable { get; set; }

        public bool IsDeletable { get; set; }
    }
}
