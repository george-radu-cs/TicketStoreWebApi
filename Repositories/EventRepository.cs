using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketStore.Entities;

namespace TicketStore.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly TicketStoreContext _db;

        public EventRepository(TicketStoreContext db)
        {
            _db = db;
        }

        public IQueryable<Event> GetEventsIQueryable()
        {
            var events = _db.Events;

            return events;
        }

        public IQueryable<Event> GetEventsWithOrganizerIQueryable()
        {
            var events = GetEventsIQueryable().Include(e => e.Organizer);

            return events;
        }

        public IQueryable<Event> GetEventsWithOrganizerAndLocationIQueryable()
        {
            var events = GetEventsWithOrganizerIQueryable().Include(e => e.Location);

            return events;
        }

        public IQueryable<Event> GetEventsWithAllDataIQueryable()
        {
            var events = GetEventsIQueryable()
                .Include(e => e.Organizer)
                .Include(e => e.Location)
                .Include(e => e.TicketTypes)
                .Include(e=>e.Guests);

            return events;
        }

        public void Create(Event e)
        {
            _db.Locations.Add(e.Location);
            _db.EventTicketTypes.Add(e.TicketTypes);
            _db.Guests.AddRange(e.Guests);
            _db.Events.Add(e);

            _db.SaveChanges();
        }

        public void Update(Event e, IEnumerable<Guest> oldGuests)
        {
            _db.Locations.Update(e.Location);
            _db.EventTicketTypes.Update(e.TicketTypes);
            _db.Guests.UpdateRange(e.Guests); // add the new guests
            _db.Events.Update(e);
            _db.Guests.RemoveRange(oldGuests); // remove the old guests
            
            _db.SaveChanges();
        }

        public void Delete(Event e)
        {
            _db.Locations.Remove(e.Location);
            _db.EventTicketTypes.Remove(e.TicketTypes);
            _db.Guests.RemoveRange(e.Guests);
            _db.Events.Remove(e);
            
            _db.SaveChanges();
        }
    }
}