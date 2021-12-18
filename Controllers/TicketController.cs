using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            this._ticketManager = ticketManager;
        }

        [HttpGet("tickets")]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var tickets = _ticketManager.GetTickets();
                return Ok(tickets);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the tickets");
            }
        }

        [HttpGet("byId/{userId}&{eventId}")]
        public async Task<IActionResult> GetTicket([FromRoute] string userId, [FromRoute] string eventId)
        {
            try
            {
                var ticket = _ticketManager.GetTicketById(userId, eventId);
                return Ok(ticket);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the ticket");
            }
        }

        [HttpGet("buyer-tickets/{userId}")]
        public async Task<IActionResult> GetBuyerTickets([FromRoute] string userId)
        {
            try
            {
                var ticket = _ticketManager.GetBuyerTickets(userId);
                return Ok(ticket);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the tickets bought by the user given");
            }
        }
        
        [HttpGet("event-tickets/{eventId}")]
        public async Task<IActionResult> GetEventTickets([FromRoute] string eventId)
        {
            try
            {
                var ticket = _ticketManager.GetEventSoldTickets(eventId);
                return Ok(ticket);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the tickets for the event given");
            }
        }

        [HttpPost("create-ticket")]
        public async Task<IActionResult> Create([FromBody] TicketModel model)
        {
            try
            {
                _ticketManager.Create(model);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("couldn't create the ticket");
            }
        }

        [HttpPatch("update-ticket")]
        public async Task<IActionResult> Update([FromBody] TicketModel model)
        {
            try
            {
                _ticketManager.Update(model);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("couldn't update the ticket");
            }
        }

        [HttpDelete("delete-ticket/{userId}&{eventId}")]
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