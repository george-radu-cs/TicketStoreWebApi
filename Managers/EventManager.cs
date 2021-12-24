using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.Repositories;
using TicketStore.Utils;
using static TicketStore.Utils.ResponseConversions;
using TicketTypes = TicketStore.Entities.TicketTypes;

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

        public (ResponseModels.EventResponseModel resEvent, string errorMessage, string errorType)
            GetEventResponseById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return (resEvent: null, errorMessage: "The Event's Id is required", errorType: ErrorTypes.UserFault);
            }

            var responseEvent = GetEventById(id);
            if (responseEvent == null)
            {
                return (resEvent: null, errorMessage: "Event not found", errorType: ErrorTypes.NotFound);
            }

            return (resEvent: ConvertToEventResponseModelWithLocationAndTicketTypesAndOrganizerAndGuests(responseEvent),
                errorMessage: null, errorType: null);
        }

        public (List<ResponseModels.EventResponseModel> resEvents, string errorMessage, string errorType) GetEvents()
        {
            // we want a list with events with their organizer
            var events = _eventRepository.GetEventsWithAllDataIQueryable()
                .Select(e => ConvertToEventResponseModelWithLocationAndTicketTypesAndOrganizerAndGuests(e))
                .ToList();

            if (events.IsNullOrEmpty())
            {
                return (resEvents: null, errorMessage: "Events not found", errorType: ErrorTypes.NotFound);
            }

            return (resEvents: events, errorMessage: null, errorType: null);
        }

        public (List<ResponseModels.EventResponseModel> resEvents, string errorMessage, string errorType)
            GetOrganizerEvents(string organizerId)
        {
            if (string.IsNullOrEmpty(organizerId))
            {
                return (resEvents: null, errorMessage: "Organizer Id is required", errorType: ErrorTypes.UserFault);
            }

            var events = _eventRepository.GetEventsWithAllDataIQueryable()
                .Where(e => e.OrganizerId == organizerId)
                .Select(e => ConvertToEventResponseModelWithLocationAndTicketTypesAndOrganizerAndGuests(e))
                .ToList();

            if (events.IsNullOrEmpty())
            {
                return (resEvents: null, errorMessage: "Events not found", errorType: ErrorTypes.NotFound);
            }

            return (resEvents: events, errorMessage: null, errorType: null);
        }

        public (bool success, string errorMessage, string errorType) Create(EventModel model)
        {
            // check if input data is valid
            var (isValid, validationErrorMessage) = Validations.ValidateEvent(model);
            if (!isValid)
            {
                return (success: false, errorMessage: validationErrorMessage, errorType: ErrorTypes.UserFault);
            }

            var newEvent = EntityConversions.ConvertToEventEntity(model);
            _eventRepository.Create(newEvent);
            return (success: true, errorMessage: null, errorType: null);
        }

        public (bool success, string errorMessage, string errorType) Update(EventModel model)
        {
            var (isValid, validationErrorMessage) = Validations.ValidateEvent(model, true);
            if (!isValid)
            {
                return (success: false, errorMessage: validationErrorMessage, errorType: ErrorTypes.UserFault);
            }

            var eventToUpdate = GetEventById(model.Id);
            if (eventToUpdate == null)
            {
                return (success: false, errorMessage: "The Event doesn't exists.", errorType: ErrorTypes.UserFault);
            }

            // get the list of old guests to remove later
            var oldGuests = eventToUpdate.Guests;
            // get the updatedEvent from the model and oldEvent
            var updatedEvent = EntityConversions.ConvertToEventEntity(model, true, eventToUpdate);

            _eventRepository.Update(updatedEvent, oldGuests);
            return (success: true, errorMessage: null, errorType: null);
        }

        public (bool success, string errorMessage, string errorType) Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return (success: false, errorMessage: "Event Id is required", errorType: ErrorTypes.UserFault);
            }

            var eventToDelete = GetEventById(id);
            if (eventToDelete == null)
            {
                return (success: false, errorMessage: "Event doesn't exists", errorType: ErrorTypes.UserFault);
            }

            _eventRepository.Delete(eventToDelete);
            return (success: true, errorMessage: null, errorType: null);
        }
    }
}