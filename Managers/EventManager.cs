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

            // TODO move later all the conversions from InputModel to EntityModel
            // get the date now for all tables
            var date = DateTime.Now.ToUniversalTime();
            var newLocation = new Location
            {
                BuildingName = model.Location.BuildingName,
                AddressFullName = model.Location.AddressFullName,
                Locality = model.Location.Locality,
                State = model.Location.State,
                StateCode = model.Location.StateCode,
                Country = model.Location.Country,
                CountryCode = model.Location.CountryCode,
                PostalCode = model.Location.PostalCode,
                Latitude = model.Location.Latitude,
                Longitude = model.Location.Longitude,
                GeocodeAccuracy = model.Location.GeocodeAccuracy,
                CreatedAt = date,
                UpdatedAt = date,
            };
            var newTicketTypes = new TicketTypes
            {
                NumberStandardTickets = model.TicketTypes.NumberStandardTickets,
                PriceStandardTicket = model.TicketTypes.PriceStandardTicket,
                NumberVipTickets = model.TicketTypes.NumberVipTickets,
                PriceVipTicket = model.TicketTypes.PriceVipTicket,
                PriceChildTicket = model.TicketTypes.PriceChildTicket,
                PriceStudentTicket = model.TicketTypes.PriceStudentTicket,
                PriceCurrency = model.TicketTypes.PriceCurrency,
                CreatedAt = date,
                UpdatedAt = date,
            };
            var newGuests = model.Guests.Select(modelGuest => new Guest
                {
                    FirstName = modelGuest.FirstName,
                    LastName = modelGuest.LastName,
                    SceneName = modelGuest.SceneName,
                    Description = modelGuest.Description,
                    Category = modelGuest.Category,
                    Genre = modelGuest.Genre,
                    Age = modelGuest.Age,
                    CreatedAt = date,
                    UpdatedAt = date,
                })
                .ToList();
            var newEvent = new Event
            {
                Name = model.Name,
                ShortName = model.ShortName,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Category = model.Category,
                Genre = model.Genre,
                OrganizerId = model.OrganizerId,
                Location = newLocation,
                TicketTypes = newTicketTypes,
                Guests = newGuests,
                CreatedAt = date,
                UpdatedAt = date,
            };

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

            // the updated date will be changed
            var updatedDate = DateTime.Now.ToUniversalTime();

            // update event's location data
            eventToUpdate.Location.BuildingName = model.Location.BuildingName;
            eventToUpdate.Location.AddressFullName = model.Location.AddressFullName;
            eventToUpdate.Location.Locality = model.Location.Locality;
            eventToUpdate.Location.State = model.Location.State;
            eventToUpdate.Location.StateCode = model.Location.StateCode;
            eventToUpdate.Location.Country = model.Location.Country;
            eventToUpdate.Location.CountryCode = model.Location.CountryCode;
            eventToUpdate.Location.PostalCode = model.Location.PostalCode;
            eventToUpdate.Location.Latitude = model.Location.Latitude;
            eventToUpdate.Location.Longitude = model.Location.Longitude;
            eventToUpdate.Location.GeocodeAccuracy = model.Location.GeocodeAccuracy;
            eventToUpdate.Location.UpdatedAt = updatedDate;

            // update ticket types data
            eventToUpdate.TicketTypes.NumberStandardTickets = model.TicketTypes.NumberStandardTickets;
            eventToUpdate.TicketTypes.PriceStandardTicket = model.TicketTypes.PriceStandardTicket;
            eventToUpdate.TicketTypes.NumberVipTickets = model.TicketTypes.NumberVipTickets;
            eventToUpdate.TicketTypes.PriceVipTicket = model.TicketTypes.PriceVipTicket;
            eventToUpdate.TicketTypes.PriceChildTicket = model.TicketTypes.PriceChildTicket;
            eventToUpdate.TicketTypes.PriceStudentTicket = model.TicketTypes.PriceStudentTicket;
            eventToUpdate.TicketTypes.PriceCurrency = model.TicketTypes.PriceCurrency;
            eventToUpdate.TicketTypes.UpdatedAt = updatedDate;

            // get the list of old guests to remove later
            var oldGuests = eventToUpdate.Guests;
            // update guests list
            eventToUpdate.Guests = model.Guests.Select(modelGuest => new Guest
            {
                FirstName = modelGuest.FirstName,
                LastName = modelGuest.LastName,
                SceneName = modelGuest.SceneName,
                Description = modelGuest.Description,
                Category = modelGuest.Category,
                Genre = modelGuest.Genre,
                Age = modelGuest.Age,
                EventId = modelGuest.EventId,
                CreatedAt = updatedDate,
                UpdatedAt = updatedDate,
            }).ToList();

            // update event data
            eventToUpdate.Name = model.Name;
            eventToUpdate.ShortName = model.ShortName;
            eventToUpdate.Description = model.Description;
            eventToUpdate.StartDate = model.StartDate;
            eventToUpdate.EndDate = model.EndDate;
            eventToUpdate.Category = model.Category;
            eventToUpdate.Genre = model.Genre;
            eventToUpdate.UpdatedAt = updatedDate;

            _eventRepository.Update(eventToUpdate, oldGuests);
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