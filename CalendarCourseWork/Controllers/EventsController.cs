using CalendarCourseWork.DataBase.Models;
using CalendarCourseWork.Logic;
using CalendarCourseWork.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarCourseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventsLogic _eventsLogic;
        private readonly UsersLogic _usersLogic;
        private readonly JWTUser _jwtUser;

        public EventsController(EventsLogic eventsLogic, UsersLogic usersLogic, JWTUser jwtUser)
        {
            _eventsLogic = eventsLogic;
            _usersLogic = usersLogic;
            _jwtUser = jwtUser;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvent(DateTime dateTimeFrom, DateTime dataTimeTo)
        {
            User user = await _usersLogic.GetCurrentUserCreds(HttpContext, _jwtUser);
            int userId = user.Id;

            return await _eventsLogic.GetEventsAsync(userId, dateTimeFrom, dataTimeTo);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            User user = await _usersLogic.GetCurrentUserCreds(HttpContext, _jwtUser);
            int userId = user.Id;

            Event @event = await _eventsLogic.GetEventByIdAsync(id, userId);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {

            User user = await _usersLogic.GetCurrentUserCreds(HttpContext, _jwtUser);

            int userId = user.Id;

            bool result = await _eventsLogic.UpdateEventAsync(id, userId, @event);

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
        public async Task<IActionResult> DeleteEvent(int id, int userId)
        {
            bool result = await _eventsLogic.DeleteEventAsync(id, userId);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
