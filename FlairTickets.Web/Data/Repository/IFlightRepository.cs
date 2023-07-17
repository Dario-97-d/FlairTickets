﻿using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        Task CreateAsync(Flight flight);
    }
}