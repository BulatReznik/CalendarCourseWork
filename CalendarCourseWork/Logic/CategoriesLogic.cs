using CalendarCourseWork.DataBase.Models;
using CalendarCourseWork.DataBase.Storages;

namespace CalendarCourseWork.Logic
{
    public class CategoriesLogic
    {
        private readonly CategoriesStorage _categoriesStorage;

        public CategoriesLogic(CategoriesStorage categoriesStorage)
        {
            _categoriesStorage = categoriesStorage;
        }

        public async Task<List<Category>> GetCategoriesAsync(int userId)
        {
            return await _categoriesStorage.GetCategoriesAsync(userId);
        }

        public async Task<Category> GetCategoryByIdAsync(int id, int userId)
        {
            return await _categoriesStorage.GetCategoryByIdAsync(id, userId);
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category category)
        {
            if (CategoryExists(category.UserId, category.Header))
            {
                return false;
            }
            return await _categoriesStorage.UpdateCategoryAsync(id, category);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            if (CategoryExists(category.UserId, category.Header)) 
            {
                return null;
            }
            return await _categoriesStorage.CreateCategoryAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(int id, int userId)
        {
            return await _categoriesStorage.DeleteCategoryAsync(id, userId);
        }

        public bool CategoryExists(int userId, string header)
        {
            return _categoriesStorage.CategoryExists(userId, header);
        }
    }
}
