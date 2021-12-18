using System.Collections.Generic;
using TicketStore.Entities;
using TicketStore.Models;

namespace TicketStore.Managers
{
    public interface ITicketManager
    {
        List<Ticket> GetTickets();
        List<Ticket> GetBuyerTickets(string userId);
        List<Ticket> GetEventSoldTickets(string eventId);
        Ticket GetTicketById(string userId, string eventId);
        void Create(TicketModel model);
        void Update(TicketModel model);
        void Delete(string userId, string eventId);
    }
}