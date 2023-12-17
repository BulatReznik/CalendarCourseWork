using CalendarCourseWork.DataBase.Models;
using CalendarCourseWork.Logic;
using CalendarCourseWork.Models;
using CalendarCourseWork.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarCourseWork.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UsersLogic _usersLogic;
        private readonly JWTUser _jwtUser;

        public UsersController(UsersLogic usersLogic, JWTUser jwtUser)
        {
            _usersLogic = usersLogic;
            _jwtUser = jwtUser;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PostUser(UserInputModel userInputModel)
        {
            try
            {
                User user = new()
                {
                    Name = userInputModel.Name,
                    Email = userInputModel.Email,
                    Password = userInputModel.Password,
                };

                User result = await _usersLogic.CreateUserAsync(user);

                if (result == null)
                {
                    return Problem("Email уже занят или не все данные были введены");
                }

                return CreatedAtAction("GetUser", new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _usersLogic.GetUsersAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            User user = await _usersLogic.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(UserInputModel userInputModel)
        {
            User user = await _usersLogic.GetCurrentUserCreds(HttpContext, _jwtUser);
            int userId = user.Id;

            bool result = await _usersLogic.UpdateUserAsync(userId, userInputModel);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool result = await _usersLogic.DeleteUserAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
