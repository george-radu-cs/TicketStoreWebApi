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
    public class TicketManager : ITicketManager
    {
        private readonly ITicketRepository _ticketRepository;

        public Ticket GetTicketById(string userId, string eventId, string auxiliaryId)
        {
            var ticket = _ticketRepository.GetTicketsWithBuyerAndEventIQueryable()
                .FirstOrDefault(t => t.UserId == userId && t.EventId == eventId && t.AuxiliaryId == auxiliaryId);

            return ticket;
        }

        public ResponseRecordWithErrors<TicketResponseModel> GetTicketResponseById(
            string userId, string eventId, string auxiliaryId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(eventId))
            {
                return new ResponseRecordWithErrors<TicketResponseModel>(null, "Id required", ErrorTypes.UserFault);
            }

            var ticket = GetTicketById(userId, eventId, auxiliaryId);
            if (ticket == null)
            {
                return new ResponseRecordWithErrors<TicketResponseModel>(null, "Ticket not found", ErrorTypes.NotFound);
            }

            return new ResponseRecordWithErrors<TicketResponseModel>(
                ConvertToTicketResponseModelWithUserAndEvent(ticket), null, null);
        }

        public TicketManager(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public ResponseRecordsListWithErrors<TicketResponseModel> GetTicketsResponse()
        {
            var tickets = _ticketRepository.GetTicketsIQueryable()
                .OrderByDescending(t => t.UpdatedAt)
                .Select(t => ConvertToTicketResponseModel(t))
                .ToList();

            if (tickets.IsNullOrEmpty())
            {
                return new ResponseRecordsListWithErrors<TicketResponseModel>(null, "Tickets not found",
                    ErrorTypes.NotFound);
            }

            return new ResponseRecordsListWithErrors<TicketResponseModel>(tickets, null, null);
        }

        public ResponseRecordsListWithErrors<TicketResponseModel> GetBuyerTicketsResponse(string userId)
        {
            var tickets = _ticketRepository.GetTicketsWithEventIQueryable()
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.UpdatedAt)
                .Select(t => ConvertToTicketResponseModelWithEvent(t))
                .ToList();

            if (tickets.IsNullOrEmpty())
            {
                return new ResponseRecordsListWithErrors<TicketResponseModel>(null, "Tickets not found",
                    ErrorTypes.NotFound);
            }

            return new ResponseRecordsListWithErrors<TicketResponseModel>(tickets, null, null);
        }

        public ResponseRecordsListWithErrors<TicketResponseModel> GetBuyerTicketsForAnEventResponse(string userId,
            string eventId)
        {
            var tickets = _ticketRepository.GetTicketsWithEventIQueryable()
                .Where(t => t.UserId == userId && t.EventId == eventId)
                .OrderByDescending(t => t.UpdatedAt)
                .Select(t => ConvertToTicketResponseModelWithEvent(t))
                .ToList();

            if (tickets.IsNullOrEmpty())
            {
                return new ResponseRecordsListWithErrors<TicketResponseModel>(null, "Tickets not found",
                    ErrorTypes.NotFound);
            }

            return new ResponseRecordsListWithErrors<TicketResponseModel>(tickets, null, null);
        }

        public ResponseRecordsListWithErrors<TicketResponseModel> GetTicketsSoldByOrganisation(string userId)
        {
            var tickets = _ticketRepository.GetTicketsWithEventIQueryable()
                .Where(t => t.Event.OrganizerId == userId)
                .OrderByDescending(t => t.UpdatedAt)
                .Select(t => ConvertToTicketResponseModelWithEvent(t))
                .ToList();

            if (tickets.IsNullOrEmpty())
            {
                return new ResponseRecordsListWithErrors<TicketResponseModel>( null, "Tickets not found", ErrorTypes.NotFound);
            }

            return new ResponseRecordsListWithErrors<TicketResponseModel>( tickets, null, null);
        }

        public ResponseRecordsListWithErrors<TicketResponseModel> GetEventSoldTicketsResponse(string eventId)
        {
            var tickets = _ticketRepository.GetTicketsWithBuyerIQueryable()
                .Where(t => t.EventId == eventId)
                .OrderByDescending(t => t.UpdatedAt)
                .Select(t => ConvertToTicketResponseModelWithUser(t))
                .ToList();

            if (tickets.IsNullOrEmpty())
            {
                return new ResponseRecordsListWithErrors<TicketResponseModel>( null, "Tickets not found", ErrorTypes.NotFound);
            }

            return new ResponseRecordsListWithErrors<TicketResponseModel>( tickets, null, null);
        }

        public ResponseSuccessWithErrors Create(TicketModel model)
        {
            var (isValid, validationErrorMessage) = Validations.ValidateTicket(model);
            if (!isValid)
            {
                return new ResponseSuccessWithErrors( false, validationErrorMessage, ErrorTypes.UserFault);
            }

            var newTicket = EntityConversions.ConvertToTicketEntity(model);
            _ticketRepository.Create(newTicket);
            return new ResponseSuccessWithErrors(true, null, null);
        }

        public ResponseSuccessWithErrors Update(TicketModel model)
        {
            var (isValid, validationErrorMessage) = Validations.ValidateTicket(model, true);
            if (!isValid)
            {
                return new ResponseSuccessWithErrors( false, validationErrorMessage, ErrorTypes.UserFault);
            }

            var ticketToUpdate = GetTicketById(model.UserId, model.EventId, model.AuxiliaryId);
            if (ticketToUpdate == null)
            {
                return new ResponseSuccessWithErrors(false, "The Ticket doesn't exists", ErrorTypes.UserFault);
            }

            var updatedTicket = EntityConversions.ConvertToTicketEntity(model, true, ticketToUpdate);
            _ticketRepository.Update(updatedTicket);
            return new ResponseSuccessWithErrors(true, null, null);
        }

        public ResponseSuccessWithErrors Delete(string userId, string eventId,
            string auxiliaryId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(eventId))
            {
                return new ResponseSuccessWithErrors(false, "Id is required", ErrorTypes.UserFault);
            }

            var ticketToDelete = GetTicketById(userId, eventId, auxiliaryId);
            if (ticketToDelete == null)
            {
                return new ResponseSuccessWithErrors(false, "The Ticket doesn't exists", ErrorTypes.UserFault);
            }

            _ticketRepository.Delete(ticketToDelete);
            return new ResponseSuccessWithErrors(true, null, null);
        }
    }
}