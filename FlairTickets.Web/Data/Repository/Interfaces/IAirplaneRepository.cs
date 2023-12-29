using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository.Interfaces
{
    public interface IAirplaneRepository : IGenericRepository<Airplane>
    {
        Task<Guid> GetPhotoGuidAsync(int airplaneId);
        Task<IEnumerable<Airplane>> GetSearchAsync(Airplane searchModel);
    }
}