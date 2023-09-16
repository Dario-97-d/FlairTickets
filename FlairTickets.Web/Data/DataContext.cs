using System.Linq;
using FlairTickets.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Airplane> Airplanes { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Make all Foreign Keys Restrict on Delete.
            var cascadeFks = builder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFks)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
            
            base.OnModelCreating(builder);
        }
    }
}
