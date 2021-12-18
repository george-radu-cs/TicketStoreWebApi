using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TicketStore.Entities
{
    public class TicketStoreContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole,
        IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public TicketStoreContext(DbContextOptions<TicketStoreContext> options) : base(options) {}
        
        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TicketTypes> EventTicketTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany(organizer => organizer.EventsCreated);

            builder.Entity<Event>()
                .HasOne(e => e.Location);

            builder.Entity<Event>()
                .HasOne(e => e.TicketTypes);
        }
    }
}