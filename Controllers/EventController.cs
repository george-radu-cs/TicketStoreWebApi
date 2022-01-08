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
                var response = _eventManager.GetEventResponseById(id);
                if (response.Record != null)
                {
                    return Ok(response.Record);
                }

                return response.ErrorType switch
                {
                    ErrorTypes.UserFault => BadRequest(
                        JsonConvert.SerializeObject(
                            $"Getting event by id failed. Error message: {response.ErrorMessage}")),
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
        public async Task<IActionResult> GetEvents(int limit = 20, int offset = 0)
        {
            try
            {
                var response = _eventManager.GetEvents(new FilterEventsOptions(limit, offset));
                if (response.HasRecords)
                {
                    return Ok(response.Records);
                }

                return response.ErrorType switch
                {
                    ErrorTypes.UserFault => BadRequest(
                        JsonConvert.SerializeObject(
                            $"Couldn't get the events. Error message: {response.ErrorMessage}")),
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
                var response = _eventManager.GetOrganizerEvents(organizerId);
                if (response.HasRecords)
                {
                    return Ok(response.Records);
                }

                return response.ErrorType switch
                {
                    ErrorTypes.UserFault => BadRequest(
                        JsonConvert.SerializeObject(
                            $"Couldn't get the organizer's events. Error message: {response.ErrorMessage}")),
                    ErrorTypes.ServerFault => StatusCode(StatusCodes.Status500InternalServerError),
                    _ => NotFound()
                };
            }
            catch (Exception e)
            {
                return BadRequest(
                    JsonConvert.SerializeObject($"couldn't get the organizer's events. Error message: {e.Message}"));
            }
        }

        [HttpPost("create-event")]
        [Authorize(Policy = AuthorizationRoles.OrganizerOrAdmin)]
        public async Task<IActionResult> Create([FromBody] EventModel eventModel)
        {
            try
            {
                var response = _eventManager.Create(eventModel);
                if (response.Success)
                {
                    return Created("success", JsonConvert.SerializeObject("Event created successfully"));
                }

                return response.ErrorType switch
                {
                    ErrorTypes.UserFault => BadRequest(
                        JsonConvert.SerializeObject(
                            $"Couldn't create the event. Error message: {response.ErrorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(
                    JsonConvert.SerializeObject($"Couldn't create the event. Error message: {e.Message}"));
            }
        }

        [HttpPatch("update-event")]
        [Authorize(Policy = AuthorizationRoles.OrganizerOrAdmin)]
        public async Task<IActionResult> Update([FromBody] EventModel eventModel)
        {
            try
            {
                var response = _eventManager.Update(eventModel);
                if (response.Success)
                {
                    return Ok(JsonConvert.SerializeObject("Event updated successfully"));
                }

                return response.ErrorType switch
                {
                    ErrorTypes.UserFault => BadRequest(
                        JsonConvert.SerializeObject(
                            $"Couldn't update the event. Error message: {response.ErrorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(
                    JsonConvert.SerializeObject($"Couldn't update the event. Error message: {e.Message}"));
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = AuthorizationRoles.OrganizerOrAdmin)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                var response = _eventManager.Delete(id);
                if (response.Success)
                {
                    return Ok(JsonConvert.SerializeObject("Event deleted successfully"));
                }

                return response.ErrorType switch
                {
                    ErrorTypes.UserFault => BadRequest(
                        JsonConvert.SerializeObject(
                            $"Couldn't delete the event. Error message: {response.ErrorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(
                    JsonConvert.SerializeObject($"Couldn't delete the event. Error message: {e.Message}"));
            }
        }
    }
}