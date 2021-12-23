using System.Collections.Generic;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.ResponseModels;

namespace TicketStore.Managers
{
    public interface IEventManager
    {
        Event GetEventById(string id);
        (EventResponseModel resEvent, string errorMessage, string errorType) GetEventResponseById(string id);
        (List<EventResponseModel> resEvents, string errorMessage, string errorType) GetEvents();

        (List<EventResponseModel> resEvents, string errorMessage, string errorType) GetOrganizerEvents(
            string organizerId);

        (bool success, string errorMessage, string errorType) Create(EventModel model);
        (bool success, string errorMessage, string errorType) Update(EventModel model);
        (bool success, string errorMessage, string errorType) Delete(string id);
    }
}