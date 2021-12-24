using System;
using System.Collections.Generic;
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

        public (TicketResponseModel resTicket, string errorMessage, string errorType) GetTicketResponseById(
            string userId, string eventId, string auxiliaryId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(eventId))
            {
                return (resTicket: null, errorMessage: "Id required", errorType: ErrorTypes.UserFault);
            }

            var ticket = GetTicketById(userId, eventId, auxiliaryId);
            if (ticket == null)
            {
                return (resTicket: null, errorMessage: "Ticket not found", errorType: ErrorTypes.NotFound);
            }

            return (resTicket: ConvertToTicketResponseModelWithUserAndEvent(ticket), errorMessage: null,
                errorType: null);
        }

        public TicketManager(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public (List<TicketResponseModel> resTickets, string errorMessage, string errorType) GetTicketsResponse()
        {
            var tickets = _ticketRepository.GetTicketsIQueryable()
                .Select(t => ConvertToTicketResponseModel(t))
                .ToList();

            if (tickets.IsNullOrEmpty())
            {
                return (resTickets: null, errorMessage: "Tickets not found", errorType: ErrorTypes.NotFound);
            }

            return (resTickets: tickets, errorMessage: null, errorType: null);
        }

        public (List<TicketResponseModel> resTickets, string errorMessage, string errorType) GetBuyerTicketsResponse(
            string userId)
        {
            var tickets = _ticketRepository.GetTicketsWithEventIQueryable()
                .Where(t => t.UserId == userId)
                .Select(t => ConvertToTicketResponseModelWithEvent(t))
                .ToList();

            if (tickets.IsNullOrEmpty())
            {
                return (resTickets: null, errorMessage: "Tickets not found", errorType: ErrorTypes.NotFound);
            }

            return (resTickets: tickets, errorMessage: null, errorType: null);
        }

        public (List<TicketResponseModel> resTickets, string errorMessage, string errorType)
            GetEventSoldTicketsResponse(string eventId)
        {
            var tickets = _ticketRepository.GetTicketsWithBuyerIQueryable()
                .Where(t => t.EventId == eventId)
                .Select(t => ConvertToTicketResponseModelWithUser(t))
                .ToList();

            if (tickets.IsNullOrEmpty())
            {
                return (resTickets: null, errorMessage: "Tickets not found", errorType: ErrorTypes.NotFound);
            }

            return (resTickets: tickets, errorMessage: null, errorType: null);
        }

        public (bool success, string errorMessage, string errorType) Create(TicketModel model)
        {
            var (isValid, validationErrorMessage) = Validations.ValidateTicket(model);
            if (!isValid)
            {
                return (success: false, errorMessage: validationErrorMessage, errorType: ErrorTypes.UserFault);
            }

            var newTicket = EntityConversions.ConvertToTicketEntity(model);
            _ticketRepository.Create(newTicket);
            return (success: true, errorMessage: null, errorType: null);
        }

        public (bool success, string errorMessage, string errorType) Update(TicketModel model)
        {
            var (isValid, validationErrorMessage) = Validations.ValidateTicket(model, true);
            if (!isValid)
            {
                return (success: false, errorMessage: validationErrorMessage, errorType: ErrorTypes.UserFault);
            }

            var ticketToUpdate = GetTicketById(model.UserId, model.EventId, model.AuxiliaryId);
            if (ticketToUpdate == null)
            {
                return (success: false, errorMessage: "The Ticket doesn't exists", errorType: ErrorTypes.UserFault);
            }

            var updatedTicket = EntityConversions.ConvertToTicketEntity(model, true, ticketToUpdate);
            _ticketRepository.Update(updatedTicket);
            return (success: true, errorMessage: null, errorType: null);
        }

        public (bool success, string errorMessage, string errorType) Delete(string userId, string eventId, string auxiliaryId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(eventId))
            {
                return (success: false, errorMessage: "Id is required", errorType: ErrorTypes.UserFault);
            }

            var ticketToDelete = GetTicketById(userId, eventId, auxiliaryId);
            if (ticketToDelete == null)
            {
                return (success: false, errorMessage: "The Ticket doesn't exists", errorType: ErrorTypes.UserFault);
            }

            _ticketRepository.Delete(ticketToDelete);
            return (success: true, errorMessage: null, errorType: null);
        }
    }
}