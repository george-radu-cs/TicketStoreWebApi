using System.Collections.Generic;
using TicketStore.Entities;
using TicketStore.Models;

namespace TicketStore.Managers
{
    public interface IEventManager
    {
        Event GetEventById(string id);
        List<Event> GetEvents();
        void Create(EventModel model);
        void Update(EventModel model);
        void Delete(string id);
    }
}