using System.Collections.Generic;
using System.Collections.ObjectModel;
using TicketStore.Entities;
using TicketStore.ResponseModels;

namespace TicketStore.Utils
{
    public static class ResponseConversions
    {
        public static UserResponseModel ConvertToUserResponseModel(User userToConvert)
        {
            return new UserResponseModel
            {
                Id = userToConvert.Id,
                FirstName = userToConvert.FirstName,
                LastName = userToConvert.LastName,
                PhoneNumber = userToConvert.PhoneNumber,
                PhonePrefix = userToConvert.PhonePrefix,
                Age = userToConvert.Age,
                IsStudent = userToConvert.IsStudent,
                JoinDate = userToConvert.CreatedAt,
                EventsCreated = new Collection<EventResponseModel>(),
                TicketsBought = new Collection<TicketResponseModel>(),
                Reviews = new Collection<ReviewResponseModel>(),
            };
        }

        private static GuestResponseModel ConvertToGuestResponseModel(Guest guestToConvert)
        {
            return new GuestResponseModel
            {
                Id = guestToConvert.Id,
                FirstName = guestToConvert.FirstName,
                LastName = guestToConvert.LastName,
                SceneName = guestToConvert.SceneName,
                Description = guestToConvert.Description,
                Category = guestToConvert.Category,
                Genre = guestToConvert.Genre,
                Age = guestToConvert.Age,
                EventId = guestToConvert.EventId,
            };
        }

        private static LocationResponseModel ConvertToLocationResponseModel(Location locationToConvert)
        {
            return new LocationResponseModel
            {
                Id = locationToConvert.Id,
                BuildingName = locationToConvert.BuildingName,
                AddressFullName = locationToConvert.AddressFullName,
                Locality = locationToConvert.Locality,
                State = locationToConvert.State,
                StateCode = locationToConvert.StateCode,
                Country = locationToConvert.Country,
                CountryCode = locationToConvert.CountryCode,
                PostalCode = locationToConvert.PostalCode,
                Latitude = locationToConvert.Latitude,
                Longitude = locationToConvert.Longitude,
                GeocodeAccuracy = locationToConvert.GeocodeAccuracy,
            };
        }

        private static TicketTypesResponseModel ConvertToTicketTypesResponseModel(TicketTypes ticketTypesToConvert)
        {
            return new TicketTypesResponseModel
            {
                Id = ticketTypesToConvert.Id,
                NumberStandardTickets = ticketTypesToConvert.NumberStandardTickets,
                PriceStandardTicket = ticketTypesToConvert.PriceStandardTicket,
                NumberVipTickets = ticketTypesToConvert.NumberVipTickets,
                PriceVipTicket = ticketTypesToConvert.PriceVipTicket,
                PriceChildTicket = ticketTypesToConvert.PriceChildTicket,
                PriceStudentTicket = ticketTypesToConvert.PriceStudentTicket,
                PriceCurrency = ticketTypesToConvert.PriceCurrency,
            };
        }

        public static EventResponseModel ConvertToEventResponseModel(Event eventToConvert)
        {
            return new EventResponseModel
            {
                Id = eventToConvert.Id,
                Name = eventToConvert.Name,
                ShortName = eventToConvert.ShortName,
                Description = eventToConvert.Description,
                StartDate = eventToConvert.StartDate,
                EndDate = eventToConvert.EndDate,
                Category = eventToConvert.Category,
                Genre = eventToConvert.Genre,
                OrganizerId = eventToConvert.OrganizerId,
                Location = null,
                TicketTypes = null,
                Organizer = null,
                Guests = new List<GuestResponseModel>()
            };
        }

        public static EventResponseModel ConvertToEventResponseModelWithLocationAndTicketTypesAndOrganizerAndGuests(
            Event eventToConvert)
        {
            var eventResponse = ConvertToEventResponseModel(eventToConvert);
            eventResponse.Location = ConvertToLocationResponseModel(eventToConvert.Location);
            eventResponse.TicketTypes = ConvertToTicketTypesResponseModel(eventToConvert.TicketTypes);
            eventResponse.Organizer = ConvertToUserResponseModel(eventToConvert.Organizer);
            eventResponse.Guests = new List<GuestResponseModel>();
            foreach (var guestToConvert in eventToConvert.Guests)
            {
                eventResponse.Guests.Add(ConvertToGuestResponseModel(guestToConvert));
            }

            return eventResponse;
        }

        public static ReviewResponseModel ConvertToReviewResponseModel(Review reviewToConvert)
        {
            return new ReviewResponseModel
            {
                UserId = reviewToConvert.UserId,
                EventId = reviewToConvert.EventId,
                Title = reviewToConvert.Title,
                Message = reviewToConvert.Message,
                Rating = reviewToConvert.Rating,
                Date = reviewToConvert.UpdatedAt,
            };
        }

        public static ReviewResponseModel ConvertToReviewResponseModelWithUser(Review reviewToConvert)
        {
            var review = ConvertToReviewResponseModel(reviewToConvert);
            review.User = ConvertToUserResponseModel(reviewToConvert.User);
            return review;
        }

        public static ReviewResponseModel ConvertToReviewResponseModelWithEvent(Review reviewToConvert)
        {
            var review = ConvertToReviewResponseModel(reviewToConvert);
            review.Event = ConvertToEventResponseModel(reviewToConvert.Event);
            return review;
        }

        public static ReviewResponseModel ConvertToReviewResponseModelWithUserAndEvent(Review reviewToConvert)
        {
            var review = ConvertToReviewResponseModelWithUser(reviewToConvert);
            review.Event = ConvertToEventResponseModel(reviewToConvert.Event);
            return review;
        }

        public static TicketResponseModel ConvertToTicketResponseModel(Ticket ticketToConvert)
        {
            return new TicketResponseModel
            {
                UserId = ticketToConvert.UserId,
                EventId = ticketToConvert.EventId,
                AuxiliaryId = ticketToConvert.AuxiliaryId,
                TicketType = ticketToConvert.TicketType,
                Price = ticketToConvert.Price,
                PriceCurrency = ticketToConvert.PriceCurrency,
                BoughtTime = ticketToConvert.UpdatedAt,
            };
        }

        public static TicketResponseModel ConvertToTicketResponseModelWithUser(Ticket ticketToConvert)
        {
            var ticket = ConvertToTicketResponseModel(ticketToConvert);
            ticket.Buyer = ConvertToUserResponseModel(ticketToConvert.User);
            return ticket;
        }

        public static TicketResponseModel ConvertToTicketResponseModelWithEvent(Ticket ticketToConvert)
        {
            var ticket = ConvertToTicketResponseModel(ticketToConvert);
            ticket.Event = ConvertToEventResponseModel(ticketToConvert.Event);
            return ticket;
        }

        public static TicketResponseModel ConvertToTicketResponseModelWithUserAndEvent(Ticket ticketToConvert)
        {
            var ticket = ConvertToTicketResponseModelWithUser(ticketToConvert);
            ticket.Event = ConvertToEventResponseModel(ticketToConvert.Event);
            return ticket;
        }
    }
}