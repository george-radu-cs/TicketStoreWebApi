using System;
using System.Collections.Generic;
using System.Linq;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.Repositories;
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
            // organizer, ticket types, location, invitates, teams
            var _event = _eventRepository.GetEventsWithAllDataIQueryable()
                .FirstOrDefault(e => e.Id == id);

            return _event;
        }

        public ResponseModels.EventResponseModel GetEventResponseById(string id)
        {
            var _event = GetEventById(id);
            return _event == null ? null : ConvertToEventResponseModelWithLocationAndTicketTypesAndOrganizer(_event);
        }

        public List<ResponseModels.EventResponseModel> GetEvents()
        {
            // we want a list with events with their organizer
            var events = _eventRepository.GetEventsWithAllDataIQueryable()
                .Select(e => ConvertToEventResponseModelWithLocationAndTicketTypesAndOrganizer(e))
                .ToList();

            return events;
        }

        public void Create(EventModel model)
        {
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
                CreatedAt = date,
                UpdatedAt = date,
            };
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
                CreatedAt = date,
                UpdatedAt = date,
            };

            _eventRepository.Create(newEvent);
        }

        public void Update(EventModel model)
        {
            var upEvent = GetEventById(model.Id);
            // the updated date will be changed
            var updatedDate = DateTime.Now.ToUniversalTime();

            // update event's location data
            upEvent.Location.BuildingName = model.Location.BuildingName;
            upEvent.Location.AddressFullName = model.Location.AddressFullName;
            upEvent.Location.Locality = model.Location.Locality;
            upEvent.Location.State = model.Location.State;
            upEvent.Location.StateCode = model.Location.StateCode;
            upEvent.Location.Country = model.Location.Country;
            upEvent.Location.CountryCode = model.Location.CountryCode;
            upEvent.Location.PostalCode = model.Location.PostalCode;
            upEvent.Location.Latitude = model.Location.Latitude;
            upEvent.Location.Longitude = model.Location.Longitude;
            upEvent.Location.GeocodeAccuracy = model.Location.GeocodeAccuracy;
            upEvent.Location.UpdatedAt = updatedDate;

            // update ticket types data
            upEvent.TicketTypes.NumberStandardTickets = model.TicketTypes.NumberStandardTickets;
            upEvent.TicketTypes.PriceStandardTicket = model.TicketTypes.PriceStandardTicket;
            upEvent.TicketTypes.NumberVipTickets = model.TicketTypes.NumberVipTickets;
            upEvent.TicketTypes.PriceVipTicket = model.TicketTypes.PriceVipTicket;
            upEvent.TicketTypes.PriceChildTicket = model.TicketTypes.PriceChildTicket;
            upEvent.TicketTypes.PriceStudentTicket = model.TicketTypes.PriceStudentTicket;
            upEvent.TicketTypes.UpdatedAt = updatedDate;

            // update event data
            upEvent.Name = model.Name;
            upEvent.ShortName = model.ShortName;
            upEvent.Description = model.Description;
            upEvent.StartDate = model.StartDate;
            upEvent.EndDate = model.EndDate;
            upEvent.Category = model.Category;
            upEvent.Genre = model.Genre;
            upEvent.UpdatedAt = updatedDate;

            _eventRepository.Update(upEvent);
        }

        public void Delete(string id)
        {
            var _event = GetEventById(id);

            _eventRepository.Delete(_event);
        }
    }
}