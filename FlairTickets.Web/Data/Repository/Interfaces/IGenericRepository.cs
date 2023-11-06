using System.Linq;
using System.Threading.Tasks;

namespace FlairTickets.Web.Data.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        Task<T> CreateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task<T> UpdateAsync(T entity);
        IQueryable<T> WhereIdEquals(int id);
    }
}