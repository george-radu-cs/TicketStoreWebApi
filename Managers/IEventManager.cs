using System.Collections.Generic;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.ResponseModels;

namespace TicketStore.Managers
{
    public interface IEventManager
    {
        Event GetEventById(string id);
        EventResponseModel GetEventResponseById(string id);
        List<EventResponseModel> GetEvents();
        void Create(EventModel model);
        void Update(EventModel model);
        void Delete(string id);
    }
}