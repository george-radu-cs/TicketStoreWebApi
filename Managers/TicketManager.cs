using System;
using System.Collections.Generic;
using System.Linq;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.Repositories;
using TicketStore.ResponseModels;
using static TicketStore.Utils.ResponseConversions;

namespace TicketStore.Managers
{
    public class TicketManager : ITicketManager
    {
        private readonly ITicketRepository _ticketRepository;

        public Ticket GetTicketById(string userId, string eventId)
        {
            var ticket = _ticketRepository.GetTicketsWithBuyerAndEventIQueryable()
                .FirstOrDefault(t => t.UserId == userId && t.EventId == eventId);

            return ticket;
        }

        public TicketResponseModel GetTicketResponseById(string userId, string eventId)
        {
            var ticket = GetTicketById(userId, eventId);

            return ConvertToTicketResponseModelWithUserAndEvent(ticket);
        }

        public TicketManager(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public List<TicketResponseModel> GetTicketsResponse()
        {
            var tickets = _ticketRepository.GetTicketsIQueryable()
                .Select(t => ConvertToTicketResponseModel(t))
                .ToList();

            return tickets;
        }

        public List<TicketResponseModel> GetBuyerTicketsResponse(string userId)
        {
            var tickets = _ticketRepository.GetTicketsWithEventIQueryable()
                .Where(t => t.UserId == userId)
                .Select(t => ConvertToTicketResponseModelWithEvent(t))
                .ToList();

            return tickets;
        }

        public List<TicketResponseModel> GetEventSoldTicketsResponse(string eventId)
        {
            var tickets = _ticketRepository.GetTicketsWithBuyerIQueryable()
                .Where(t => t.EventId == eventId)
                .Select(t => ConvertToTicketResponseModelWithUser(t))
                .ToList();

            return tickets;
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
            var updatedTicket = GetTicketById(model.UserId, model.EventId);
            var date = DateTime.Now.ToUniversalTime();
            updatedTicket.TicketType = model.TicketType;
            updatedTicket.Price = model.Price;
            updatedTicket.UpdatedAt = date;
            
            _ticketRepository.Update(updatedTicket);
        }

        public void Delete(string userId, string eventId)
        {
            var ticket = GetTicketById(userId, eventId);
            
            _ticketRepository.Delete(ticket);
        }
    }
}