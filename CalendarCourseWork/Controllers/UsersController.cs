using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarCourseWork.BusinessLogic.Models;
using CalendarCourseWork.Logic;
using CalendarCourseWork.Security;
using CalendarCourseWork.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CalendarCourseWork.Controllers
{
    public class UsersController : Controller
    {

        private readonly UsersManager _usersLogic;
        private readonly JWTUser _jwtUser;

        public UsersController(UsersManager usersLogic, JWTUser jwtUser)
        {
            _usersLogic = usersLogic;
            _jwtUser = jwtUser;
        }

        // GET: Users/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<IActionResult> Register(UserInputModel userInputModel)
        {
            if (!ModelState.IsValid)
            {
                // Если модель не валидна, вернуть представление с ошибками валидации
                return View(userInputModel);
            }

            User user = new()
            {
                Name = userInputModel.Name,
                Email = userInputModel.Email,
                Password = userInputModel.Password,
            };

            User result = await _usersLogic.CreateUserAsync(user);

            return RedirectToAction(nameof(Login));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit()
        {
            User user = await _usersLogic.GetCurrentUserCreds(HttpContext, _jwtUser);
            int userId = user.Id;

            if (userId == 0)
            {
                return NotFound();
            }

            User userInfo = await _usersLogic.GetUserByIdAsync(userId);

            if (userInfo == null)
            {
                return NotFound();
            }

            // Создаем объект UserInputModel и заполняем его данными из базы
            UserInputModel userInputModel = new()
            {
                Name = userInfo.Name,
                Email = userInfo.Email,
                Password = userInfo.Password,
            };

            return View(userInputModel);
        }

        // POST: Users/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserInputModel userInputModel)
        {
            User user = await _usersLogic.GetCurrentUserCreds(HttpContext, _jwtUser);
            int userId = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    // Обновляем данные пользователя
                    await _usersLogic.UpdateUserAsync(userId, userInputModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_usersLogic.UserExists(user.Email))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userInputModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(JWTInputModel userInputModel)
        {
            User user = new()
            {
                Email = userInputModel.Email,
                Password = userInputModel.Password,
            };

            ClaimsIdentity identity = await _jwtUser.GetIdentityAsync(user);

            if (identity != null)
            {
                Claim? userIdClaim = identity.FindFirst("Id");
                int userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                // Перенаправление на страницу
                return RedirectToAction("MonthWithEvents", "Calendar", new { year = DateTime.Now.Year, month = DateTime.Now.Month, userId = userId });
            }

            return RedirectToAction("UserWas");
        }

        public IActionResult UserWas()
        {
            return View();
        }
    }
}
