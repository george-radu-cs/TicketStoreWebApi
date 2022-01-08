using System.Linq;
using Castle.Core.Internal;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.Repositories;
using TicketStore.ResponseModels;
using TicketStore.Utils;
using static TicketStore.Utils.ResponseConversions;

namespace TicketStore.Managers
{
    public class EventManager : IEventManager
    {
        private readonly IEventRepository _eventRepository;

        public EventManager(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Event GetEventById(string id)
        {
            // when getting an event by id get all info about an event
            // organizer, ticket types, location, guests, teams
            var resEvent = _eventRepository.GetEventsWithAllDataIQueryable()
                .FirstOrDefault(e => e.Id == id);

            return resEvent;
        }

        public ResponseRecordWithErrors<EventResponseModel> GetEventResponseById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new ResponseRecordWithErrors<EventResponseModel>(null, "The Event's Id is required",
                    ErrorTypes.UserFault);
            }

            var responseEvent = GetEventById(id);
            if (responseEvent == null)
            {
                return new ResponseRecordWithErrors<EventResponseModel>(null, "Event not found", ErrorTypes.NotFound);
            }

            return new ResponseRecordWithErrors<EventResponseModel>(
                ConvertToEventResponseModelWithLocationAndTicketTypesAndOrganizerAndGuests(responseEvent), null, null);
        }

        // public (List<EventResponseModel> resEvents, string errorMessage, string errorType)
        public ResponseRecordsListWithErrors<EventResponseModel> GetEvents(FilterEventsOptions filterEventsOptions)
        {
            // we want a list with events with their organizer
            var events = _eventRepository.GetEventsWithAllDataIQueryable()
                .OrderByDescending(e => e.UpdatedAt)
                .Skip(filterEventsOptions.Offset)
                .Take(filterEventsOptions.Limit)
                .Select(e => ConvertToEventResponseModelWithLocationAndTicketTypesAndOrganizerAndGuests(e))
                .ToList();

            if (events.IsNullOrEmpty())
            {
                return new ResponseRecordsListWithErrors<EventResponseModel>(null, "Events not found",
                    ErrorTypes.NotFound);
            }

            return new ResponseRecordsListWithErrors<EventResponseModel>(events, null, null);
        }

        public ResponseRecordsListWithErrors<EventResponseModel> GetOrganizerEvents(string organizerId)
        {
            if (string.IsNullOrEmpty(organizerId))
            {
                return new ResponseRecordsListWithErrors<EventResponseModel>(null, "Organizer Id is required",
                    ErrorTypes.UserFault);
            }

            var events = _eventRepository.GetEventsWithAllDataIQueryable()
                .Where(e => e.OrganizerId == organizerId)
                .OrderByDescending(e => e.UpdatedAt)
                .Select(e => ConvertToEventResponseModelWithLocationAndTicketTypesAndOrganizerAndGuests(e))
                .ToList();

            if (events.IsNullOrEmpty())
            {
                return new ResponseRecordsListWithErrors<EventResponseModel>(null, "Events not found",
                    ErrorTypes.NotFound);
            }

            return new ResponseRecordsListWithErrors<EventResponseModel>(events, null, null);
        }

        public ResponseSuccessWithErrors Create(EventModel model)
        {
            // check if input data is valid
            var (isValid, validationErrorMessage) = Validations.ValidateEvent(model);
            if (!isValid)
            {
                return new ResponseSuccessWithErrors(false, validationErrorMessage, ErrorTypes.UserFault);
            }

            var newEvent = EntityConversions.ConvertToEventEntity(model);
            _eventRepository.Create(newEvent);
            return  new ResponseSuccessWithErrors(true, null, null);
        }

        public ResponseSuccessWithErrors Update(EventModel model)
        {
            var (isValid, validationErrorMessage) = Validations.ValidateEvent(model, true);
            if (!isValid)
            {
                return new ResponseSuccessWithErrors(false, validationErrorMessage, ErrorTypes.UserFault);
            }

            var eventToUpdate = GetEventById(model.Id);
            if (eventToUpdate == null)
            {
                return new ResponseSuccessWithErrors(false, "The Event doesn't exists.", ErrorTypes.UserFault);
            }

            // get the list of old guests to remove later
            var oldGuests = eventToUpdate.Guests;
            // get the updatedEvent from the model and oldEvent
            var updatedEvent = EntityConversions.ConvertToEventEntity(model, true, eventToUpdate);

            _eventRepository.Update(updatedEvent, oldGuests);
            return new ResponseSuccessWithErrors(true, null, null);
        }

        public ResponseSuccessWithErrors Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new ResponseSuccessWithErrors(false, "Event Id is required", ErrorTypes.UserFault);
            }

            var eventToDelete = GetEventById(id);
            if (eventToDelete == null)
            {
                return new ResponseSuccessWithErrors( false, "Event doesn't exists", ErrorTypes.UserFault);
            }

            _eventRepository.Delete(eventToDelete);
            return new ResponseSuccessWithErrors(true, null, null);
        }
    }
}