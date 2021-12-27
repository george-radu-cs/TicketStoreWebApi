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
    public class EventController : ControllerBase
    {
        private readonly IEventManager _eventManager;

        public EventController(IEventManager eventEventManager)
        {
            _eventManager = eventEventManager;
        }

        [HttpGet("byId/{id}")]
        [Authorize(Policy = AuthorizationRoles.Anyone)]
        public async Task<IActionResult> GetEvent([FromRoute] string id)
        {
            try
            {
                var (resEvent, errorMessage, errorType) = _eventManager.GetEventResponseById(id);
                if (resEvent != null)
                {
                    return Ok(resEvent);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Getting event by id failed. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault => StatusCode(StatusCodes.Status500InternalServerError),
                    _ => NotFound()
                };
            }
            catch (Exception e)
            {
                return NotFound(JsonConvert.SerializeObject($"Couldn't get the event. Error message: {e.Message}"));
            }
        }

        [HttpGet("events")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var (resEvents, errorMessage, errorType) = _eventManager.GetEvents();
                if (resEvents != null)
                {
                    return Ok(resEvents);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't get the events. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault => StatusCode(StatusCodes.Status500InternalServerError),
                    _ => NotFound()
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the events. Error message: {e.Message}"));
            }
        }

        [HttpGet("organizer-events/{organizerId}")]
        [Authorize(Policy = AuthorizationRoles.OrganizerOrAdmin)]
        public async Task<IActionResult> GetOrganizerEvents([FromRoute] string organizerId)
        {
            try
            {
                var (resEvents, errorMessage, errorType) = _eventManager.GetOrganizerEvents(organizerId);
                if (resEvents != null)
                {
                    return Ok(resEvents);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't get the organizer's events. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault => StatusCode(StatusCodes.Status500InternalServerError),
                    _ => NotFound()
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"couldn't get the organizer's events. Error message: {e.Message}"));
            }
        }

        [HttpPost("create-event")]
        [Authorize(Policy = AuthorizationRoles.OrganizerOrAdmin)]
        public async Task<IActionResult> Create([FromBody] EventModel eventModel)
        {
            try
            {
                var (success, errorMessage, errorType) = _eventManager.Create(eventModel);
                if (success)
                {
                    return Created("success", JsonConvert.SerializeObject("Event created successfully"));
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't create the event. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't create the event. Error message: {e.Message}"));
            }
        }

        [HttpPatch("update-event")]
        [Authorize(Policy = AuthorizationRoles.OrganizerOrAdmin)]
        public async Task<IActionResult> Update([FromBody] EventModel eventModel)
        {
            try
            {
                var (success, errorMessage, errorType) = _eventManager.Update(eventModel);
                if (success)
                {
                    return Ok(JsonConvert.SerializeObject("Event updated successfully"));
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't update the event. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't update the event. Error message: {e.Message}"));
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = AuthorizationRoles.OrganizerOrAdmin)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                var (success, errorMessage, errorType) = _eventManager.Delete(id);
                if (success)
                {
                    return Ok(JsonConvert.SerializeObject("Event deleted successfully"));
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't delete the event. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't delete the event. Error message: {e.Message}"));
            }
        }
    }
}