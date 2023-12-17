using CalendarCourseWork.DataBase.Models;
using CalendarCourseWork.Logic;
using CalendarCourseWork.Models;
using CalendarCourseWork.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarCourseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesLogic _categoriesLogic;
        private readonly UsersLogic _usersLogic;
        private readonly JWTUser _jwtUser;

        public CategoriesController(CategoriesLogic categoriesLogic, UsersLogic usersLogic, JWTUser jwtUser)
        {
            _categoriesLogic = categoriesLogic;
            _usersLogic = usersLogic;
            _jwtUser = jwtUser;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            User user = await _usersLogic.GetCurrentUserCreds(HttpContext, _jwtUser);

            int userId = user.Id;

            return await _categoriesLogic.GetCategoriesAsync(userId);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryOutputModel>> GetCategory(int id)
        {
            User user = await _usersLogic.GetCurrentUserCreds(HttpContext, _jwtUser);

            int userId = user.Id;

            Category category = await _categoriesLogic.GetCategoryByIdAsync(id, userId);

            CategoryOutputModel outputModel = new()
            {
                EmailSend = category.EmailSend,
                Header = category.Header,
                UserId = userId,
                Id = userId,
            };

            if (outputModel == null)
            {
                return NotFound();
            }

            return outputModel;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryInputModel categoryInputModel)
        {
            User user = await _usersLogic.GetCurrentUserCreds(HttpContext, _jwtUser);

            Category category = new()
            {
                EmailSend = categoryInputModel.EmailSend,
                Header = categoryInputModel.Header,
                UserId = user.Id
            };

            bool result = await _categoriesLogic.UpdateCategoryAsync(id, category);

            if (!result)
            {
                return BadRequest("Уже есть категория с таким именем");
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CategoryInputModel categoryInputModel)
        {
            User user = await _usersLogic.GetCurrentUserCreds(HttpContext, _jwtUser);

            if (user == null)
            {
                return Problem("Не удалось найти пользователя");
            }

            Category category = new()
            {
                EmailSend = categoryInputModel.EmailSend,
                Header = categoryInputModel.Header,
                UserId = user.Id
            };

            Category result = await _categoriesLogic.CreateCategoryAsync(category);

            if (result == null)
            {
                return Problem("Не удалось добавить категорию, возможно уже есть категория с таким именем");
            }

            return CreatedAtAction("GetCategory", new { id = result.Id }, result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            User user = await _usersLogic.GetCurrentUserCreds(HttpContext, _jwtUser);

            int userId = user.Id;

            bool result = await _categoriesLogic.DeleteCategoryAsync(id, userId);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
