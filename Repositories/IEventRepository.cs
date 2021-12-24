using System.Collections.Generic;
using System.Linq;
using TicketStore.Entities;

namespace TicketStore.Repositories
{
    public interface IEventRepository
    {
        IQueryable<Event> GetEventsIQueryable();
        IQueryable<Event> GetEventsWithOrganizerIQueryable();
        IQueryable<Event> GetEventsWithOrganizerAndLocationIQueryable();
        IQueryable<Event> GetEventsWithAllDataIQueryable();
        void Create(Event e);
        void Update(Event e, IEnumerable<Guest> oldGuests);
        void Delete(Event e);
    }
}