using System.Linq;
using TicketStore.Entities;

namespace TicketStore.Repositories
{
    public interface ITicketRepository
    {
        IQueryable<Ticket> GetTicketsIQueryable();
        IQueryable<Ticket> GetTicketsWithBuyerIQueryable();
        IQueryable<Ticket> GetTicketsWithEventIQueryable();
        IQueryable<Ticket> GetTicketsWithBuyerAndEventIQueryable();
        void Create(Ticket t);
        void Update(Ticket t);
        void Delete(Ticket t);
    }
}