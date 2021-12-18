using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketStore.Entities;

namespace TicketStore.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketStoreContext _db;

        public TicketRepository(TicketStoreContext db)
        {
            this._db = db;
        }

        public IQueryable<Ticket> GetTicketsIQueryable()
        {
            var tickets = _db.Tickets;

            return tickets;
        }

        public IQueryable<Ticket> GetTicketsWithBuyerIQueryable()
        {
            var tickets = _db.Tickets.Include(t => t.User);

            return tickets;
        }

        public IQueryable<Ticket> GetTicketsWithEventIQueryable()
        {
            var tickets = _db.Tickets.Include(t => t.Event);

            return tickets;
        }

        public IQueryable<Ticket> GetTicketsWithBuyerAndEventIQueryable()
        {
            var tickets = _db.Tickets
                .Include(t => t.User)
                .Include(t => t.Event);

            return tickets;
        }

        public void Create(Ticket t)
        {
            _db.Tickets.Add(t);
            
            _db.SaveChanges();
        }

        public void Update(Ticket t)
        {
            _db.Tickets.Update(t);

            _db.SaveChanges();
        }

        public void Delete(Ticket t)
        {
            _db.Tickets.Remove(t);

            _db.SaveChanges();
        }
    }
}