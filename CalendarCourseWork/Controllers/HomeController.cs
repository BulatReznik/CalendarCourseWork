using CalendarCourseWork.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalendarCourseWork.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
