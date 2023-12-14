using CalendarCourseWork.DataBase.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalendarCourseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventsLogic _eventsLogic;

        public EventsController(EventsLogic eventsLogic)
        {
            _eventsLogic = eventsLogic;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvent()
        {
            return await _eventsLogic.GetEventsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            Event @event = await _eventsLogic.GetEventByIdAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            bool result = await _eventsLogic.UpdateEventAsync(id, @event);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            Event result = await _eventsLogic.CreateEventAsync(@event);

            if (result == null)
            {
                return Problem("Entity set 'CalendarCourseWorkContext.Event' is null.");
            }

            return CreatedAtAction("GetEvent", new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            bool result = await _eventsLogic.DeleteEventAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
