using System;
using System.Collections.Generic;
using System.Linq;
using TicketStore.Entities;
using TicketStore.Models;

namespace TicketStore.Utils
{
    public static class EntityConversions
    {
        // in this convert methods we set the default parameters: isEdit to false and oldEntity to null to keep the call
        // of the methods nice when converting a new entity. Instructions to call these methods to be sure no exception
        // is thrown:
        // 1) If calling for a new entity only the modelToConvert is needed
        // 2) If calling for an existing entity that we want to update, set the modelToConvert with the new data and
        // additionally set isEdit to true & set the oldEntity to the entity we want to update from db

        public static User ConvertToUserEntity(SignUpUserModel userToConvert)
        {
            var date = DateTime.Now.ToUniversalTime();
            return new User
            {
                FirstName = userToConvert.FirstName,
                LastName = userToConvert.LastName,
                Email = userToConvert.Email,
                UserName = userToConvert.Email,
                PhoneNumber = userToConvert.PhoneNumber,
                PhonePrefix = userToConvert.PhonePrefix,
                Age = userToConvert.Age,
                IsStudent = userToConvert.IsStudent,
                CreatedAt = date,
                UpdatedAt = date,
            };
        }

        private static Location ConvertToLocationEntity(LocationModel locationToConvert, DateTime date,
            bool isEdit = false, Location oldLocation = null)
        {
            var resLocation = oldLocation;
            if (!isEdit)
            {
                resLocation = new Location { CreatedAt = date };
            }

            if (resLocation == null) // occurs when isEdit=true and oldLocation wasn't provided
            {
                throw new ArgumentNullException(nameof(oldLocation));
            }

            resLocation.BuildingName = locationToConvert.BuildingName;
            resLocation.AddressFullName = locationToConvert.AddressFullName;
            resLocation.Locality = locationToConvert.Locality;
            resLocation.State = locationToConvert.State;
            resLocation.StateCode = locationToConvert.StateCode;
            resLocation.Country = locationToConvert.Country;
            resLocation.CountryCode = locationToConvert.CountryCode;
            resLocation.PostalCode = locationToConvert.PostalCode;
            resLocation.Latitude = locationToConvert.Latitude;
            resLocation.Longitude = locationToConvert.Longitude;
            resLocation.GeocodeAccuracy = locationToConvert.GeocodeAccuracy;
            resLocation.UpdatedAt = date;

            return resLocation;
        }

        private static TicketTypes ConvertToTicketTypesEntity(TicketTypesModels ticketTypesToConvert, DateTime date,
            bool isEdit = false, TicketTypes oldTicketTypes = null)
        {
            var resTicketTypes = oldTicketTypes;
            if (!isEdit)
            {
                resTicketTypes = new TicketTypes { CreatedAt = date };
            }

            if (resTicketTypes == null) // occurs when isEdit=true and oldTicketTypes wasn't provided
            {
                throw new ArgumentNullException(nameof(oldTicketTypes));
            }

            resTicketTypes.NumberStandardTickets = ticketTypesToConvert.NumberStandardTickets;
            resTicketTypes.PriceStandardTicket = ticketTypesToConvert.PriceStandardTicket;
            resTicketTypes.NumberVipTickets = ticketTypesToConvert.NumberVipTickets;
            resTicketTypes.PriceVipTicket = ticketTypesToConvert.PriceVipTicket;
            resTicketTypes.PriceChildTicket = ticketTypesToConvert.PriceChildTicket;
            resTicketTypes.PriceStudentTicket = ticketTypesToConvert.PriceStudentTicket;
            resTicketTypes.PriceCurrency = ticketTypesToConvert.PriceCurrency;
            resTicketTypes.UpdatedAt = date;

            return resTicketTypes;
        }

        private static List<Guest> ConvertToListOfGuestsEntity(IEnumerable<GuestModel> guestsToConvert, DateTime date)
        {
            return guestsToConvert.Select(modelGuest => new Guest
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
        }

        // if isEdit is true then oldEvent parameter must be provided, otherwise an exception of null reference
        // will be raised, by default isEdit will be false and oldEvent will be null
        public static Event ConvertToEventEntity(EventModel eventToConvert, bool isEdit = false, Event oldEvent = null)
        {
            var date = DateTime.Now.ToUniversalTime();

            // initialize the response event with old event
            var resEvent = oldEvent;

            if (!isEdit) // if not editing oldEvent will be null, so initialize the new event and set the CreatedAt
            {
                resEvent = new Event { CreatedAt = date };
            }

            if (resEvent == null) // occurs when isEdit=true and oldEvent wasn't provided
            {
                throw new ArgumentNullException(nameof(oldEvent));
            }

            resEvent.Name = eventToConvert.Name;
            resEvent.ShortName = eventToConvert.ShortName;
            resEvent.Description = eventToConvert.Description;
            resEvent.StartDate = eventToConvert.StartDate;
            resEvent.EndDate = eventToConvert.EndDate;
            resEvent.Category = eventToConvert.Category;
            resEvent.Genre = eventToConvert.Genre;
            resEvent.OrganizerId = eventToConvert.OrganizerId;
            resEvent.Location = ConvertToLocationEntity(eventToConvert.Location, date, isEdit, resEvent.Location);
            resEvent.TicketTypes =
                ConvertToTicketTypesEntity(eventToConvert.TicketTypes, date, isEdit, resEvent.TicketTypes);
            resEvent.Guests = ConvertToListOfGuestsEntity(eventToConvert.Guests, date);
            resEvent.UpdatedAt = date;

            return resEvent;
        }

        public static Review ConvertToReviewEntity(ReviewModel reviewToConvert, bool isEdit = false,
            Review oldReview = null)
        {
            var date = DateTime.Now.ToUniversalTime();
            var resReview = oldReview;
            if (!isEdit)
            {
                resReview = new Review { CreatedAt = date };
            }

            if (resReview == null) // occurs when isEdit=true and oldReview wasn't provided
            {
                throw new ArgumentNullException(nameof(oldReview));
            }

            resReview.Title = reviewToConvert.Title;
            resReview.Message = reviewToConvert.Message;
            resReview.Rating = reviewToConvert.Rating;
            resReview.UserId = reviewToConvert.UserId;
            resReview.EventId = reviewToConvert.EventId;
            resReview.UpdatedAt = date;

            return resReview;
        }

        public static Ticket ConvertToTicketEntity(TicketModel ticketToConvert, bool isEdit = false,
            Ticket oldTicket = null)
        {
            var date = DateTime.Now.ToUniversalTime();
            var resTicket = oldTicket;
            if (!isEdit)
            {
                resTicket = new Ticket { CreatedAt = date };
            }

            if (resTicket == null) // occurs when isEdit=true and oldTicket wasn't provided
            {
                throw new ArgumentNullException(nameof(oldTicket));
            }

            resTicket.UserId = ticketToConvert.UserId;
            resTicket.EventId = ticketToConvert.EventId;
            resTicket.TicketType = ticketToConvert.TicketType;
            resTicket.Price = ticketToConvert.Price;
            resTicket.UpdatedAt = date;

            return resTicket;
        }
    }
}