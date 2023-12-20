using Microsoft.AspNetCore.Mvc;
using CalendarCourseWork.BusinessLogic.Models;
using CalendarCourseWork.Logic;
using CalendarCourseWork.Models;

namespace CalendarCourseWork.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoriesManager _categoriesLogic;
        private readonly UsersManager _usersLogic;

        public CategoriesController(CategoriesManager categoriesLogic, UsersManager usersLogic)
        {
            _categoriesLogic = categoriesLogic;
            _usersLogic = usersLogic;
        }

        // GET: Categories1
        public async Task<IActionResult> Index(int userId)
        {
            var result = _categoriesLogic.GetCategoriesAsync(userId);

            ViewBag.userId = userId;

            return View(await result);
        }

        // GET: Categories/Create
        public IActionResult Create(int userId)
        {
            var model = new CategoryInputModel
            {
                UserId = userId
            };

            return View(model);
        }

        // POST: Categories/Create
        [HttpPost]
        public async Task<IActionResult> Create(int userId, CategoryInputModel categoryInputModel)
        {
            if (ModelState.IsValid)
            {
                Category category = new()
                {
                    EmailSend = categoryInputModel.EmailSend,
                    Header = categoryInputModel.Header,
                    UserId = userId
                };

                await _categoriesLogic.CreateCategoryAsync(category);

                return RedirectToAction(nameof(Index), new { userId });
            }
            return View(categoryInputModel);
        }

        // GET: Categories1/Edit/5
        public async Task<IActionResult> Edit(int id, int userId)
        {
            Category category = await _categoriesLogic.GetCategoryByIdAsync(id, userId);

            return View(category);
        }

        // POST: Categories1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryInputModel categoryInputModel, int userId)
        {
            if (ModelState.IsValid)
            {
                Category category = new()
                {
                    EmailSend = categoryInputModel.EmailSend,
                    Header = categoryInputModel.Header,
                    UserId = userId
                };

                bool result = await _categoriesLogic.UpdateCategoryAsync(id, category);

                return RedirectToAction(nameof(Index));
            }
            return View(categoryInputModel);
        }

        /*

        // GET: Categories1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Category == null)
            {
                return Problem("Entity set 'CalendarCourseWorkContext.Category'  is null.");
            }
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        */
    }
}
