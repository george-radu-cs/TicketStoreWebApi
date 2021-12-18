using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketStore.Managers;
using TicketStore.Models;

namespace TicketStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventManager _eventManager;

        public EventController(IEventManager eventEventManager)
        {
            this._eventManager = eventEventManager;
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var events = _eventManager.GetEvents();
                return Ok(events);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the events");
            }
        }

        [HttpGet("byId/{id}")]
        public async Task<IActionResult> GetEvent([FromRoute] string id)
        {
            try
            {
                var _event = _eventManager.GetEventById(id);
                return Ok(_event);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the event");
            }
        }

        [HttpPost("create-event")]
        public async Task<IActionResult> Create([FromBody] EventModel eventModel)
        {
            try
            {
                _eventManager.Create(eventModel);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("couldn't create event");
            }
        }

        [HttpPatch("update-event")]
        public async Task<IActionResult> Update([FromBody] EventModel eventModel)
        {
            try
            {
                _eventManager.Update(eventModel);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("couldn't update the event");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                _eventManager.Delete(id);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("couldn't delete the event");
            }
        }
    }
}