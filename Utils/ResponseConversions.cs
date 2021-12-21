using System;
using System.Collections.ObjectModel;
using TicketStore.Entities;
using TicketStore.ResponseModels;

namespace TicketStore.Utils
{
    public static class ResponseConversions
    {
        private static UserResponseModel ConvertToUserResponseModel(User userToConvert)
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
                BoughtTime = ticketTypesToConvert.UpdatedAt,
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
                Location = null,
                TicketTypes = null,
                Organizer = null,
            };
        }

        public static EventResponseModel ConvertToEventResponseModelWithLocationAndTicketTypesAndOrganizer(
            Event eventToConvert)
        {
            var eventResponse = ConvertToEventResponseModel(eventToConvert);
            eventResponse.Location = ConvertToLocationResponseModel(eventToConvert.Location);
            eventResponse.TicketTypes = ConvertToTicketTypesResponseModel(eventToConvert.TicketTypes);
            eventResponse.Organizer = ConvertToUserResponseModel(eventToConvert.Organizer);
            return eventResponse;
        }

        public static ReviewResponseModel ConvertToReviewResponseModel(Review reviewToConvert)
        {
            return new ReviewResponseModel
            {
                Id = reviewToConvert.Id,
                Title = reviewToConvert.Title,
                Message = reviewToConvert.Message,
                Rating = reviewToConvert.Rating,
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
                TicketType = ticketToConvert.TicketType,
                Price = ticketToConvert.Price,
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