﻿using System.Collections.Generic;
using FlairTickets.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlairTickets.Web.Data.Repository.Interfaces
{
    public interface IAirportRepository : IGenericRepository<Airport>
    {
        IEnumerable<SelectListItem> GetComboAirports();
    }
}