using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.ResponseModels;

namespace TicketStore.Managers
{
    public interface IEventManager
    {
        Event GetEventById(string id);
        ResponseRecordWithErrors<EventResponseModel> GetEventResponseById(string id);
        ResponseRecordsListWithErrors<EventResponseModel> GetEvents(FilterEventsOptions filterEventsOptions);
        ResponseRecordsListWithErrors<EventResponseModel> GetOrganizerEvents(string organizerId);
        ResponseSuccessWithErrors Create(EventModel model);
        ResponseSuccessWithErrors Update(EventModel model);
        ResponseSuccessWithErrors Delete(string id);
    }
}