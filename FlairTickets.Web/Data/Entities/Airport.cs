using FlairTickets.Web.Data.Repository.Interfaces;

namespace FlairTickets.Web.Data.Entities
{
    public class Airport : IEntity
    {
        public int Id { get; set; }

        public string IataCode { get; set; }

        public string Name { get; set; }

        public string City { get; set; }
        
        public string Country { get; set; }

        public string ComboName => $"{IataCode} - {City}";
    }
}
