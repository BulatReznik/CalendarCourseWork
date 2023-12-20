using CalendarCourseWork.BusinessLogic.Models;
using CalendarCourseWork.Logic;
using CalendarCourseWork.Models;
using CalendarCourseWork.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CalendarCourseWork.Controllers
{
    public class CalendarController : Controller
    {
        private readonly EventsManager _eventsLogic;
        private readonly UsersManager _usersLogic;
        private readonly JWTUser _jwtUser;

        public CalendarController(EventsManager eventsLogic, UsersManager usersLogic, JWTUser jwtUser)
        {
            _eventsLogic = eventsLogic;
            _usersLogic = usersLogic;
            _jwtUser = jwtUser;
        }

        public async Task<IActionResult> MonthWithEvents(int year, int month, int userId)
        {
            DateTime firstDayOfMonth = new(year, month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            List<Event> events = await GetEvents(userId, firstDayOfMonth, lastDayOfMonth);

            CalendarViewModel viewModel = new()
            {
                FirstDayOfMonth = firstDayOfMonth,
                Events = events,
                UserId = userId
            };

            ViewBag.CalendarViewModel = viewModel;

            return View();
        }

        public async Task<IActionResult> Year(int year, int month, int userId)
        {

            DateTime firstDayOfMonth = new(year, month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            List<Event> events = await GetEvents(userId, firstDayOfMonth, lastDayOfMonth);

            CalendarViewModel viewModel = new()
            {
                FirstDayOfMonth = firstDayOfMonth,
                Events = events,
                UserId = userId
            };

            ViewBag.CalendarViewModel = viewModel;

            return View();
        }

        public async Task<List<Event>> GetEvents(int userId, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            return await _eventsLogic.GetEventsAsync(userId, dateTimeFrom, dateTimeTo);
        }
    }
}
