using CalendarCourseWork.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarCourseWork.DataBase.Storages
{
    public class CategoriesStorage
    {
        private readonly CalendarCourseWorkContext _context;

        public CategoriesStorage(CalendarCourseWorkContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            if (_context.Category == null)
            {
                return new List<Category>();
            }

            return await _context.Category.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            if (_context.Category == null)
            {
                return null;
            }

            return await _context.Category.FindAsync(id);
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category category)
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
                // Handle the case where the entity set is null
                return null;
            }

            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            if (_context.Category == null)
            {
                return false;
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return false;
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
