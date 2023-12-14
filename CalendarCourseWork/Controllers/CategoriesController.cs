using CalendarCourseWork.DataBase.Models;
using CalendarCourseWork.Logic;
using Microsoft.AspNetCore.Mvc;

namespace CalendarCourseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesLogic _categoriesLogic;

        public CategoriesController(CategoriesLogic categoriesLogic)
        {
            _categoriesLogic = categoriesLogic;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            return await _categoriesLogic.GetCategoriesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            Category category = await _categoriesLogic.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            bool result = await _categoriesLogic.UpdateCategoryAsync(id, category);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            Category result = await _categoriesLogic.CreateCategoryAsync(category);

            if (result == null)
            {
                return Problem("Entity set 'CalendarCourseWorkContext.Category' is null.");
            }

            return CreatedAtAction("GetCategory", new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            bool result = await _categoriesLogic.DeleteCategoryAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
