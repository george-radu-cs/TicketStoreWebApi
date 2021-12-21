using System.Collections.Generic;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.ResponseModels;

namespace TicketStore.Managers
{
    public interface ITicketManager
    {
        Ticket GetTicketById(string userId, string eventId);
        TicketResponseModel GetTicketResponseById(string userId, string eventId);
        List<TicketResponseModel> GetTicketsResponse();
        List<TicketResponseModel> GetBuyerTicketsResponse(string userId);
        List<TicketResponseModel> GetEventSoldTicketsResponse(string eventId);
        void Create(TicketModel model);
        void Update(TicketModel model);
        void Delete(string userId, string eventId);
    }
}