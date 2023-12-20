using CalendarCourseWork.BusinessLogic.Models;
using CalendarCourseWork.Logic;
using CalendarCourseWork.Security;
using Microsoft.AspNetCore.Mvc;
using Hangfire;

namespace CalendarCourseWork.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsManager _eventsManager;
        private readonly UsersManager _usersManager;
        private readonly JWTUser _jwtUser;
        private readonly CategoriesManager _categoriesManager;

        public EventsController(CategoriesManager categoriesManager, EventsManager eventsManager, UsersManager usersManager, JWTUser jwtUser)
        {
            _categoriesManager = categoriesManager;
            _eventsManager = eventsManager;
            _usersManager = usersManager;
            _jwtUser = jwtUser;
        }


        // GET: Events/Create
        public async Task<IActionResult> Create(int userId, int year, int month, int day)
        {
            var model = new Event();

            ViewBag.Categories = await _categoriesManager.GetCategoriesAsync(userId);

            DateTime selectedDate = new(year, month, day);
            ViewBag.Date = selectedDate;

            return View(model);
        }


        // POST: Events/Create
        [HttpPost]
        public async Task<IActionResult> Create(int userId, Event @event)
        {
            if (ModelState.IsValid)
            {
                await _eventsManager.CreateEventAsync(@event);

                DateTime dateTimeFrom = new(@event.EventTime.Year, @event.EventTime.Month, @event.EventTime.Day, 0, 0, 0);

                DateTime dateTimeTo = new(@event.EventTime.Year, @event.EventTime.Month, @event.EventTime.Day, 23, 59, 59);

                // Запланируйте задачу на отправку уведомления в указанное время
                BackgroundJob.Schedule(() => SendNotification(@event, userId), @event.EventTime);

                return RedirectToAction(nameof(Index), new { userId, dateTimeFrom, dateTimeTo });
            }
            return View(@event);
        }

        // GET: Events
        public async Task<IActionResult> Index(int userId, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var result = _eventsManager.GetEventsAsync(userId, dateTimeFrom, dateTimeTo);

            ViewBag.Date = dateTimeFrom;

            ViewBag.UserId = userId;

            return View(await result);
        }

        public async Task<ActionResult<IEnumerable<Event>>> GetEvent(DateTime dateTimeFrom, DateTime dataTimeTo)
        {
            User user = await _usersManager.GetCurrentUserCreds(HttpContext, _jwtUser);
            int userId = user.Id;

            return await _eventsManager.GetEventsAsync(userId, dateTimeFrom, dataTimeTo);
        }

        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            User user = await _usersManager.GetCurrentUserCreds(HttpContext, _jwtUser);
            int userId = user.Id;

            Event @event = await _eventsManager.GetEventByIdAsync(id, userId);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        public async Task<IActionResult> PutEvent(int id, Event @event)
        {

            User user = await _usersManager.GetCurrentUserCreds(HttpContext, _jwtUser);

            int userId = user.Id;

            bool result = await _eventsManager.UpdateEventAsync(id, userId, @event);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {

            Event result = await _eventsManager.CreateEventAsync(@event);

            if (result == null)
            {
                return Problem("Entity set 'CalendarCourseWorkContext.Event' is null.");
            }

            return CreatedAtAction("GetEvent", new { id = result.Id }, result);
        }

        public async Task<IActionResult> DeleteEvent(int id, int userId)
        {
            bool result = await _eventsManager.DeleteEventAsync(id, userId);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Метод для отправки уведомлений
        /// </summary>
        /// <param name="event"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task SendNotification(Event @event, int userId)
        {
            // Получите пользователя по userId
            User user = await _usersManager.GetUserByIdAsync(userId);

            // Создайте экземпляр EmailService
            IEmailService emailService = new EmailService();

            // Отправьте уведомление
            string to = user.Email;
            string subject = "Напоминание: " + @event.EventName; // Заголовок вашего уведомления
            string html = $"Уважаемый {user.Name},<br/><br/> Напоминаем вам о событии: {@event.EventName} в {@event.EventTime}. Описание события: {@event.EventDescription}";
            string from = "7777bulat7777@gmail.com"; // Замените на свой адрес электронной почты

            emailService.SendEmail(to, subject, html, from);
        }
    }
}
