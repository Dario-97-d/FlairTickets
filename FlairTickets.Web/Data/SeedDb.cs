using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            //await _context.SaveChangesAsync();
        }
    }
}
