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

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoriesStorage.GetCategoriesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoriesStorage.GetCategoryByIdAsync(id);
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category category)
        {
            return await _categoriesStorage.UpdateCategoryAsync(id, category);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            return await _categoriesStorage.CreateCategoryAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _categoriesStorage.DeleteCategoryAsync(id);
        }

        public bool CategoryExists(int id)
        {
            return _categoriesStorage.CategoryExists(id);
        }
    }
}
