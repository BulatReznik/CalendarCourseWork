using CalendarCourseWork.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarCourseWork.BusinessLogic.Storages
{
    public class CategoriesDataAccess
    {
        private readonly CalendarCourseWorkContext _context;

        public CategoriesDataAccess()
        {

        }

        public CategoriesDataAccess(CalendarCourseWorkContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Category>> GetCategoriesAsync(int userId)
        {
            if (_context.Category == null)
            {
                return new List<Category>();
            }

            // Используйте ToListAsync для поддержки асинхронных операций
            List<Category> userCategories = await _context.Category
                .Where(category => category.UserId == userId)
                .ToListAsync();

            return userCategories;
        }

        public virtual async Task<Category> GetCategoryByIdAsync(int id, int userId)
        {
            if (_context.Category == null)
            {
                return null;
            }

            Category category = await _context.Category.FindAsync(id);

            // Проверяем, принадлежит ли категория указанному пользователю
            if (category != null && category.UserId == userId)
            {
                return category;
            }

            return null; // Категория не найдена или не принадлежит указанному пользователю
        }

        public virtual async Task<bool> UpdateCategoryAsync(int id, Category category)
        {
            if (id != category.Id)
            {
                return false;
            }

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            if (_context.Category == null)
            {
                return null;
            }

            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int id, int userId)
        {
            if (_context.Category == null)
            {
                return false;
            }

            // Проверяем, существует ли категория с данным id
            Category category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return false; // Категория не найдена
            }

            // Проверяем, имеет ли пользователь доступ к данной категории
            if (category.UserId != userId)
            {
                return false; // У пользователя нет доступа к этой категории
            }

            // Удаляем категорию и сохраняем изменения
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return true; // Категория успешно удалена
        }

        public virtual bool CategoryExists(int userId, string header)
        {
            return (_context.Category?.Any(e => e.UserId == userId && e.Header == header)).GetValueOrDefault();
        }
    }
}
