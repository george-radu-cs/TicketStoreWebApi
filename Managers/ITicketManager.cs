using System.Collections.Generic;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.ResponseModels;

namespace TicketStore.Managers
{
    public interface ITicketManager
    {
        Ticket GetTicketById(string userId, string eventId, string auxiliaryId);

        (TicketResponseModel resTicket, string errorMessage, string errorType) GetTicketResponseById(string userId,
            string eventId, string auxiliaryId);

        (List<TicketResponseModel> resTickets, string errorMessage, string errorType) GetTicketsResponse();

        (List<TicketResponseModel> resTickets, string errorMessage, string errorType) GetBuyerTicketsResponse(
            string userId);

        (List<TicketResponseModel> resTickets, string errorMessage, string errorType) GetBuyerTicketsForAnEventResponse(
            string userId, string eventId);

        (List<TicketResponseModel> resTickets, string errorMessage, string errorType) GetTicketsSoldByOrganisation(
            string userId);

        (List<TicketResponseModel> resTickets, string errorMessage, string errorType) GetEventSoldTicketsResponse(
            string eventId);

        (bool success, string errorMessage, string errorType) Create(TicketModel model);
        (bool success, string errorMessage, string errorType) Update(TicketModel model);
        (bool success, string errorMessage, string errorType) Delete(string userId, string eventId, string auxiliaryId);
    }
}