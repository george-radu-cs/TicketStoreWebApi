using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TicketStore.Entities
{
    public class TicketStoreContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole,
        IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public TicketStoreContext(DbContextOptions<TicketStoreContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TicketTypes> EventTicketTypes { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Review> Reviews { get; set; }

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

            builder.Entity<Event>()
                .HasMany(e => e.Guests)
                .WithOne(i => i.Event);

            builder.Entity<Ticket>().HasKey(t => new { t.UserId, t.EventId, t.AuxiliaryId });

            builder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId);

            builder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.TicketsSold)
                .HasForeignKey(t => t.EventId);

            builder.Entity<Review>().HasKey(r => new { r.UserId, r.EventId });

            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            builder.Entity<Review>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Reviews)
                .HasForeignKey(r => r.EventId);
        }
    }
}