using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketStore.Entities;
using TicketStore.Managers;
using TicketStore.Models;

namespace TicketStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketManager _ticketManager;

        public TicketController(ITicketManager ticketManager)
        {
            _ticketManager = ticketManager;
        }

        [HttpGet("tickets")]
        [Authorize(Policy = AuthorizationRoles.Admin)]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var tickets = _ticketManager.GetTicketsResponse();
                return Ok(tickets);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the tickets");
            }
        }

        [HttpGet("byId/{userId}&{eventId}")]
        [Authorize(Policy = AuthorizationRoles.Anyone)]
        public async Task<IActionResult> GetTicket([FromRoute] string userId, [FromRoute] string eventId)
        {
            try
            {
                var ticket = _ticketManager.GetTicketResponseById(userId, eventId);
                return Ok(ticket);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the ticket");
            }
        }

        [HttpGet("buyer-tickets/{userId}")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> GetBuyerTickets([FromRoute] string userId)
        {
            try
            {
                var tickets = _ticketManager.GetBuyerTicketsResponse(userId);
                return Ok(tickets);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the tickets bought by the user given");
            }
        }
        
        [HttpGet("event-tickets/{eventId}")]
        [Authorize(Policy = AuthorizationRoles.OrganizerOrAdmin)]
        public async Task<IActionResult> GetEventTickets([FromRoute] string eventId)
        {
            try
            {
                var tickets = _ticketManager.GetEventSoldTicketsResponse(eventId);
                return Ok(tickets);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the tickets for the event given");
            }
        }

        [HttpPost("create-ticket")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Create([FromBody] TicketModel ticketModel)
        {
            try
            {
                _ticketManager.Create(ticketModel);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("couldn't create the ticket");
            }
        }

        [HttpPatch("update-ticket")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Update([FromBody] TicketModel ticketModel)
        {
            try
            {
                _ticketManager.Update(ticketModel);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("couldn't update the ticket");
            }
        }

        [HttpDelete("delete-ticket/{userId}&{eventId}")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Delete([FromRoute] string userId, [FromRoute] string eventId)
        {
            try
            {
                _ticketManager.Delete(userId, eventId);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("couldn't delete the ticket");
            }
        }
    }
}