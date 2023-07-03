using System.Linq;
using System.Threading.Tasks;

namespace FlairTickets.Web.Data.Repository
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
    }
}