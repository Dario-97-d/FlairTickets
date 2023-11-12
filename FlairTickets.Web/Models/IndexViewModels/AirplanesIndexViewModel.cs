using System.Collections.Generic;
using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Models.IndexViewModels
{
    public class AirplanesIndexViewModel
    {
        public Airplane SearchModel { get; set; }

        public IEnumerable<Airplane> Airplanes { get; set; }
    }
}
