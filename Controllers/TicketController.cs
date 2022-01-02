using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketStore.Entities;
using TicketStore.Managers;
using TicketStore.Models;
using TicketStore.Utils;

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

        [HttpGet("byId/{userId}&{eventId}&{auxiliaryId}")]
        [Authorize(Policy = AuthorizationRoles.Anyone)]
        public async Task<IActionResult> GetTicket([FromRoute] string userId, string eventId, string auxiliaryId)
        {
            try
            {
                var (resTicket, errorMessage, errorType) =
                    _ticketManager.GetTicketResponseById(userId, eventId, auxiliaryId);
                if (resTicket != null)
                {
                    return Ok(resTicket);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Getting ticket by id failed. Error message: {errorMessage}")),
                    ErrorTypes.NotFound => NotFound(),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError),
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the ticket. Error message: {e.Message}"));
            }
        }

        [HttpGet("tickets")]
        [Authorize(Policy = AuthorizationRoles.Admin)]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var (resTickets, errorMessage, errorType) = _ticketManager.GetTicketsResponse();
                if (resTickets != null)
                {
                    return Ok(resTickets);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't get the tickets. Error message: {errorMessage}")),
                    ErrorTypes.NotFound => NotFound(),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError),
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the tickets. Error message: {e.Message}"));
            }
        }

        [HttpGet("buyer-tickets/{userId}")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> GetBuyerTickets([FromRoute] string userId)
        {
            try
            {
                var (resTickets, errorMessage, errorType) = _ticketManager.GetBuyerTicketsResponse(userId);
                if (resTickets != null)
                {
                    return Ok(resTickets);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't get the user's tickets. Error message: {errorMessage}")),
                    ErrorTypes.NotFound => NotFound(),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError),
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the tickets bought by the user given. Error message: {e.Message}"));
            }
        }
        
        [HttpGet("buyer-tickets-for-event/{userId}&{eventId}")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> GetBuyerTicketsForAnEvent([FromRoute] string userId, string eventId)
        {
            try
            {
                var (resTickets, errorMessage, errorType) = _ticketManager.GetBuyerTicketsForAnEventResponse(userId, eventId);
                if (resTickets != null)
                {
                    return Ok(resTickets);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't get the user's tickets. Error message: {errorMessage}")),
                    ErrorTypes.NotFound => NotFound(),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError),
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the tickets bought by the user given. Error message: {e.Message}"));
            }
        }

        [HttpGet("tickets-sold-by-org/{userId}")] [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> GetTicketsSoldByOrganisation([FromRoute] string userId)
        {
            try
            {
                var (resTickets, errorMessage, errorType) = _ticketManager.GetTicketsSoldByOrganisation(userId);
                if (resTickets != null)
                {
                    return Ok(resTickets);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't get the tickets sold. Error message: {errorMessage}")),
                    ErrorTypes.NotFound => NotFound(),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError),
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the tickets sold by the organizer given. Error message: {e.Message}"));
            }
        }

        [HttpGet("event-tickets/{eventId}")]
        [Authorize(Policy = AuthorizationRoles.OrganizerOrAdmin)]
        public async Task<IActionResult> GetEventTickets([FromRoute] string eventId)
        {
            try
            {
                var (resTickets, errorMessage, errorType) = _ticketManager.GetEventSoldTicketsResponse(eventId);
                if (resTickets != null)
                {
                    return Ok(resTickets);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't get the event tickets. Error message: {errorMessage}")),
                    ErrorTypes.NotFound => NotFound(),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError),
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the tickets for the event given. Error message: {e.Message}"));
            }
        }

        [HttpPost("create-ticket")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Create([FromBody] TicketModel ticketModel)
        {
            try
            {
                var (success, errorMessage, errorType) = _ticketManager.Create(ticketModel);
                if (success)
                {
                    return Created("success", JsonConvert.SerializeObject("Ticket created successfully"));
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't create the ticket. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't create the ticket. Error message: {e.Message}"));
            }
        }

        [HttpPatch("update-ticket")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Update([FromBody] TicketModel ticketModel)
        {
            try
            {
                var (success, errorMessage, errorType) = _ticketManager.Update(ticketModel);
                if (success)
                {
                    return Ok(JsonConvert.SerializeObject("Ticket updated successfully"));
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't update the ticket. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't update the ticket. Error message: {e.Message}"));
            }
        }

        [HttpDelete("delete-ticket/{userId}&{eventId}&{auxiliaryId}")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Delete([FromRoute] string userId, string eventId, string auxiliaryId)
        {
            try
            {
                var (success, errorMessage, errorType) = _ticketManager.Delete(userId, eventId, auxiliaryId);
                if (success)
                {
                    return Ok(JsonConvert.SerializeObject("Ticket deleted successfully"));
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't delete the ticket. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't delete the ticket. Error message: {e.Message}"));
            }
        }
    }
}