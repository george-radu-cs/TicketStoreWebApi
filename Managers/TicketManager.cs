using System;
using System.Collections.Generic;
using System.Linq;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.Repositories;

namespace TicketStore.Managers
{
    public class TicketManager : ITicketManager
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketManager(ITicketRepository ticketRepository)
        {
            this._ticketRepository = ticketRepository;
        }

        public List<Ticket> GetTickets()
        {
            var tickets = _ticketRepository.GetTicketsIQueryable()
                .Select(t => t)
                .ToList();

            return tickets;
        }

        public List<Ticket> GetBuyerTickets(string userId)
        {
            var tickets = _ticketRepository.GetTicketsWithEventIQueryable()
                .Where(t => t.UserId == userId)
                .Select(t => t)
                .ToList();

            return tickets;
        }

        public List<Ticket> GetEventSoldTickets(string eventId)
        {
            var tickets = _ticketRepository.GetTicketsWithBuyerIQueryable()
                .Where(t => t.EventId == eventId)
                .Select(t => t)
                .ToList();

            return tickets;
        }

        public Ticket GetTicketById(string userId, string eventId)
        {
            var ticket = _ticketRepository.GetTicketsWithBuyerAndEventIQueryable()
                .FirstOrDefault(t => t.UserId == userId && t.EventId == eventId);

            return ticket;
        }

        public void Create(TicketModel model)
        {
            var date = DateTime.Now.ToUniversalTime();
            var newTicket = new Ticket
            {
                UserId = model.UserId,
                EventId = model.EventId,
                TicketType = model.TicketType,
                Price = model.Price,
                CreatedAt = date,
                UpdatedAt = date,
            };
            
            _ticketRepository.Create(newTicket);
        }

        public void Update(TicketModel model)
        {
            var updateTicket = GetTicketById(model.UserId, model.EventId);
            var date = DateTime.Now.ToUniversalTime();
            updateTicket.UserId = model.UserId;
            updateTicket.EventId = model.EventId;
            updateTicket.TicketType = model.TicketType;
            updateTicket.Price = model.Price;
            updateTicket.UpdatedAt = date;
            
            _ticketRepository.Update(updateTicket);
        }

        public void Delete(string userId, string eventId)
        {
            var ticket = GetTicketById(userId, eventId);
            
            _ticketRepository.Delete(ticket);
        }
    }
}