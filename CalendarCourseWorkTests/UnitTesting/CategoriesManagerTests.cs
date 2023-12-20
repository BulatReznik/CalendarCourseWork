using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CalendarCourseWork.BusinessLogic;
using CalendarCourseWork.BusinessLogic.Models;
using CalendarCourseWork.BusinessLogic.Storages;
using CalendarCourseWork.Logic;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore.InMemory;


namespace CalendarCourseWorkTests.UnitTesting
{
    [TestFixture]
    public class CategoriesDataAccessTests
    {
        [Test]
        public async Task GetCategoriesAsync_ReturnsUserCategories()
        {
            var mockCategoriesDataAccess = new Mock<CategoriesDataAccess>();
            var categoriesManager = new CategoriesManager(mockCategoriesDataAccess.Object);

            mockCategoriesDataAccess.Setup(m => m.GetCategoryByIdAsync(19, 32))
                .ReturnsAsync(new Category { EmailSend = true, Header = "Категория 1", UserId = 32, Id = 19 });

            var resultCategories = await categoriesManager.GetCategoryByIdAsync(19, 32);

            Assert.That(resultCategories, Is.Not.Null);
            Assert.That(resultCategories.Id, Is.EqualTo(19));
            Assert.That(resultCategories.Header, Is.EqualTo("Категория 1"));
            Assert.That(resultCategories.UserId, Is.EqualTo(32));
        }

        [Test]
        public async Task GetCategoriesAsync_ReturnsCategoriesByUser()
        {

            // Создание mock DbSet<Category>
            var categoryDbSetMock = new Mock<DbSet<Category>>();

            // Создание фиксированного списка категорий
            var categories = new List<Category>
            {
                new Category { Id = 1, Header = "Категория 1", UserId = 32 },
                new Category { Id = 2, Header = "Категория 2", UserId = 32 },
                new Category { Id = 3, Header = "Категория 3", UserId = 32 },
                new Category { Id = 4, Header = "Категория 4", UserId = 32 }
            };


            var options = new DbContextOptionsBuilder<CalendarCourseWorkContext>()
               .UseInMemoryDatabase(databaseName: "CalendarCourseWorkContext")
               .Options;

            using (var context = new CalendarCourseWorkContext(options))
            {
                context.Category.AddRange(categories);
                context.SaveChanges();
            }

            using (var context = new CalendarCourseWorkContext(options))
            {
                CategoriesDataAccess categoriesDataAccess = new(context);

                // Действие: вызов метода GetCategoriesAsync
                var resultCategories = await categoriesDataAccess.GetCategoriesAsync(32);

                // Проверка результатов
                Assert.That(resultCategories.Count, Is.EqualTo(4));
                Assert.That(resultCategories.Any(c => c.Header == "Категория 1"), Is.True);
                Assert.That(resultCategories.Any(c => c.Header == "Категория 2"), Is.True);
                Assert.That(resultCategories.Any(c => c.Header == "Категория 3"), Is.True);
                Assert.That(resultCategories.Any(c => c.Header == "Категория 4"), Is.True);
            }
        }
    }
}
