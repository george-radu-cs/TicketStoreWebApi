using System;
using System.Text.RegularExpressions;
using Castle.Core.Internal;
using TicketStore.Models;

namespace TicketStore.Utils
{
    public static class Validations
    {
        private static readonly Regex EmailRegex = new(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$");
        private static readonly Regex NameRegex = new(@"^[a-zA-Z ]+$");
        private static readonly Regex PhoneNumberRegex = new(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$");
        private static readonly Regex RatingRegex = new(@"^[0-9](\.[0-9]{1,2})?$");
        private static readonly Regex PositiveIntegerRegex = new(@"^[0-9]*$");
        private static readonly Regex PositiveFloatRegex = new(@"^[^\-][0-9]*.?[0-9]{1,2}$");

        public static (bool isValid, string errorMessage) ValidateRegister(SignUpUserModel userToValidate)
        {
            if (userToValidate.FirstName.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The FirstName is required.");
            }

            if (userToValidate.FirstName.Length is < 3 or > 21)
            {
                return (isValid: false,
                    errorMessage: "The FirstName must have at least 3 letters and at most 21.");
            }

            if (!NameRegex.IsMatch(userToValidate.FirstName))
            {
                return (isValid: false, errorMessage: "The FirstName must contain only letters.");
            }

            if (userToValidate.LastName.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The LastName is required.");
            }

            if (userToValidate.LastName.Length is < 3 or > 21)
            {
                return (isValid: false,
                    errorMessage: "The LastName must have at least 3 letters and at most 21.");
            }

            if (!NameRegex.IsMatch(userToValidate.LastName))
            {
                return (isValid: false, errorMessage: "The LastName must contain only letters.");
            }

            if (userToValidate.Email.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Email is required.");
            }

            if (!EmailRegex.IsMatch(userToValidate.Email))
            {
                return (isValid: false, errorMessage: "The Email is invalid.");
            }

            if (userToValidate.PhoneNumber.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The PhoneNumber is required.");
            }

            if (!PhoneNumberRegex.IsMatch(userToValidate.PhoneNumber))
            {
                return (isValid: false, errorMessage: "The PhoneNumber is invalid.");
            }

            if (userToValidate.PhonePrefix.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The PhonePrefix is required.");
            }

            if (userToValidate.Age <= 0)
            {
                return (isValid: false, errorMessage: "The Age must be a positive integer.");
            }

            if (userToValidate.Password.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Password is required.");
            }

            if (userToValidate.Password.Length < 6)
            {
                return (isValid: false, errorMessage: "The Password must have at least 6 chars.");
            }

            if (userToValidate.Role.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Role is required.");
            }

            if (userToValidate.Role != "Buyer" && userToValidate.Role != "Organizer")
            {
                return (isValid: false,
                    errorMessage: "The Role is invalid. Valid types are Buyer and Organizer");
            }

            return (isValid: true, errorMessage: null);
        }

        public static (bool isValid, string errorMessage) ValidateLogin(LoginUserModel userToValidate)
        {
            if (userToValidate.Email.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Email is required.");
            }

            if (!EmailRegex.IsMatch(userToValidate.Email))
            {
                return (isValid: false, errorMessage: "The Email is invalid.");
            }

            if (userToValidate.Password.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Password is required.");
            }


            return (isValid: true, errorMessage: null);
        }

        private static (bool isValid, string errorMessage) ValidateGuest(GuestModel guestToValidate,
            bool isEdit = false)
        {
            if (guestToValidate.FirstName.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The FirstName is required.");
            }

            if (!NameRegex.IsMatch(guestToValidate.FirstName))
            {
                return (isValid: false, errorMessage: "The FirstName must contain only letters.");
            }

            if (guestToValidate.LastName.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The LastName is required.");
            }

            if (!NameRegex.IsMatch(guestToValidate.LastName))
            {
                return (isValid: false, errorMessage: "The LastName must contain only letters.");
            }

            if (guestToValidate.SceneName.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The SceneName is required.");
            }

            if (guestToValidate.Description.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Description is required.");
            }

            if (guestToValidate.Category.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Category is required.");
            }

            if (guestToValidate.Genre.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Genre is required");
            }

            if (guestToValidate.Age <= 0)
            {
                return (isValid: false, errorMessage: "The Age must be a positive integer.");
            }

            if (isEdit && guestToValidate.EventId.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The EventId is required.");
            }

            return (isValid: true, errorMessage: null);
        }

        private static (bool isValid, string errorMessage) ValidateLocation(LocationModel locationToValidate)
        {
            if (locationToValidate.BuildingName.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "Location The BuildingName is required.");
            }

            if (locationToValidate.AddressFullName.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "Location The AddressFullName is required.");
            }

            if (locationToValidate.Locality.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "Location The State is required.");
            }

            if (locationToValidate.StateCode.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "Location The StateCode is required.");
            }

            if (locationToValidate.Country.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "Location The Country is required.");
            }

            if (locationToValidate.CountryCode.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "Location The CountryCode is required.");
            }

            if (locationToValidate.PostalCode.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "Location The PostalCode is required.");
            }

            if (locationToValidate.Latitude == 0)
            {
                return (isValid: false, errorMessage: "Location The Latitude is required.");
            }

            if (locationToValidate.Longitude == 0)
            {
                return (isValid: false, errorMessage: "Location The Longitude is required.");
            }

            if (locationToValidate.GeocodeAccuracy.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "Location The GeocodeAccuracy is required.");
            }

            return (isValid: true, errorMessage: null);
        }

        private static (bool isValid, string errorMessage) ValidateTicketTypes(TicketTypesModels ticketTypesToValidate)
        {
            if (ticketTypesToValidate.NumberStandardTickets <= 0)
            {
                return (isValid: false,
                    errorMessage: "TicketTypes The Number of Standard Tickets must be a positive integer.");
            }

            if (ticketTypesToValidate.PriceStandardTicket.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "TicketTypes The PriceStandardTicket is required.");
            }

            if (!PositiveFloatRegex.IsMatch(ticketTypesToValidate.PriceStandardTicket))
            {
                return (isValid: false, errorMessage: "TicketTypes The PriceStandardTicket is invalid.");
            }

            // an event doesn't need to sell vip tickets, or other types of tickets
            if (ticketTypesToValidate.NumberVipTickets < 0)
            {
                return (isValid: false, errorMessage: "TicketTypes The Number of Vip Tickets cannot be negative.");
            }

            // the price of other types of tickets (other than standard) can have the value null to represent that the
            // event won't sell these types of tickets, so we'll check only for null
            if (ticketTypesToValidate.NumberVipTickets > 0)
            {
                if (ticketTypesToValidate.PriceVipTicket.IsNullOrEmpty())
                {
                    return (isValid: false, errorMessage: "TicketTypes The PriceVipTicket is required.");
                }

                if (!PositiveFloatRegex.IsMatch(ticketTypesToValidate.PriceVipTicket))
                {
                    return (isValid: false, errorMessage: "TicketTypes The PriceVipTicket is invalid.");
                }
            }

            if (!ticketTypesToValidate.PriceChildTicket.IsNullOrEmpty() &&
                !PositiveFloatRegex.IsMatch(ticketTypesToValidate.PriceChildTicket))
            {
                return (isValid: false, errorMessage: "TicketTypes The PriceChildTicket is invalid.");
            }

            if (!ticketTypesToValidate.PriceStudentTicket.IsNullOrEmpty() &&
                !PositiveFloatRegex.IsMatch(ticketTypesToValidate.PriceStudentTicket))
            {
                return (isValid: false, errorMessage: "TicketTypes The PriceStudentTicket is invalid.");
            }

            if (ticketTypesToValidate.PriceCurrency.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "TicketTypes The PriceCurrency is required.");
            }

            // the only accepted currencies are USD, EUR, GBT & RON
            if (ticketTypesToValidate.PriceCurrency != "USD" && ticketTypesToValidate.PriceCurrency != "EUR" &&
                ticketTypesToValidate.PriceCurrency != "GBT" && ticketTypesToValidate.PriceCurrency != "RON")
            {
                return (isValid: false,
                    errorMessage: "TicketTypes The PriceCurrency is invalid. Valid types are: USD, EUR, GBT and RON");
            }


            return (isValid: true, errorMessage: null);
        }

        public static (bool isValid, string errorMessage) ValidateEvent(EventModel eventToValidate, bool isEdit = false)
        {
            if (isEdit && eventToValidate.Id.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Id is required.");
            }

            if (eventToValidate.Name.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Name is required.");
            }

            if (eventToValidate.ShortName.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The ShortName is required");
            }

            if (eventToValidate.Description.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Description is required");
            }

            if (eventToValidate.StartDate < DateTime.Now)
            {
                return (isValid: false, errorMessage: "The StartDate cannot be in the past");
            }

            if (eventToValidate.StartDate > eventToValidate.EndDate)
            {
                return (isValid: false, errorMessage: "The StartDate cannot be after the EndDate");
            }

            if (eventToValidate.Category.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Category is required");
            }

            if (eventToValidate.Genre.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Genre is required");
            }

            if (eventToValidate.OrganizerId.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The OrganizerId is required");
            }

            var (isLocationValid, locationErrorMessage) = ValidateLocation(eventToValidate.Location);
            if (!isLocationValid)
            {
                return (isValid: false, errorMessage: locationErrorMessage);
            }

            var (isTicketTypesValid, ticketTypesErrorMessage) = ValidateTicketTypes(eventToValidate.TicketTypes);
            if (!isTicketTypesValid)
            {
                return (isValid: false, errorMessage: ticketTypesErrorMessage);
            }

            foreach (var guestToValidate in eventToValidate.Guests)
            {
                var (isGuestValid, guestErrorMessage) = ValidateGuest(guestToValidate, isEdit);
                if (!isGuestValid)
                {
                    return (isValid: false, errorMessage: guestErrorMessage);
                }
            }

            // the event is valid
            return (isValid: true, errorMessage: null);
        }

        public static (bool isValid, string errorMessage) ValidateTicket(TicketModel ticketToValidate,
            bool isEdit = false)
        {
            if (ticketToValidate.UserId.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The UserId is required");
            }

            if (ticketToValidate.EventId.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The EventId is required");
            }

            if (isEdit && ticketToValidate.AuxiliaryId.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The AuxiliaryId is required");
            }

            if (ticketToValidate.TicketType.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The TicketType is required");
            }

            // ticket type can be STANDARD, VIP, CHILD, STUDENT
            if (ticketToValidate.TicketType != "STANDARD" && ticketToValidate.TicketType != "VIP" &&
                ticketToValidate.TicketType != "CHILD" && ticketToValidate.TicketType != "STUDENT")
            {
                return (isValid: false,
                    errorMessage: "The TicketType is invalid. Valid types are: STANDARD, VIP, CHILD and STUDENT");
            }

            if (ticketToValidate.Price.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Price is required");
            }

            if (!PositiveFloatRegex.IsMatch(ticketToValidate.Price))
            {
                return (isValid: false, errorMessage: "The Price is invalid.");
            }

            if (ticketToValidate.PriceCurrency.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The PriceCurrency is required.");
            }

            // the only accepted currencies are USD, EUR, GBT & RON
            if (ticketToValidate.PriceCurrency != "USD" && ticketToValidate.PriceCurrency != "EUR" &&
                ticketToValidate.PriceCurrency != "GBT" && ticketToValidate.PriceCurrency != "RON")
            {
                return (isValid: false,
                    errorMessage: "The PriceCurrency is invalid. Valid types are: USD, EUR, GBT and RON");
            }

            return (isValid: true, errorMessage: null);
        }

        public static (bool isValid, string errorMessage) ValidateReview(ReviewModel reviewToValidate,
            bool isEdit = false)
        {
            if (reviewToValidate.UserId.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The UserId is required.");
            }

            if (reviewToValidate.EventId.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The EventId is required.");
            }

            if (reviewToValidate.Title.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Title is required.");
            }

            if (reviewToValidate.Message.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Message is required.");
            }

            if (reviewToValidate.Rating.IsNullOrEmpty())
            {
                return (isValid: false, errorMessage: "The Rating is required.");
            }

            if (float.Parse(reviewToValidate.Rating) is < 0 or > 5)
            {
                return (isValid: false,
                    errorMessage:
                    "The Rating must be a string's representation of a float with 2 digits in the range [0,5].");
            }

            if (!RatingRegex.IsMatch(reviewToValidate.Rating))
            {
                return (isValid: false, errorMessage: "The Rating is invalid. Valid types: 1, 6.9, 4.20");
            }

            return (isValid: true, errorMessage: null);
        }
    }
}