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
                .ReturnsAsync(new Category { EmailSend = true, Header = "��������� 1", UserId = 32, Id = 19 });

            var resultCategories = await categoriesManager.GetCategoryByIdAsync(19, 32);

            Assert.That(resultCategories, Is.Not.Null);
            Assert.That(resultCategories.Id, Is.EqualTo(19));
            Assert.That(resultCategories.Header, Is.EqualTo("��������� 1"));
            Assert.That(resultCategories.UserId, Is.EqualTo(32));
        }

        [Test]
        public async Task GetCategoriesAsync_ReturnsCategoriesByUser()
        {

            // �������� mock DbSet<Category>
            var categoryDbSetMock = new Mock<DbSet<Category>>();

            // �������� �������������� ������ ���������
            var categories = new List<Category>
            {
                new Category { Id = 1, Header = "��������� 1", UserId = 32 },
                new Category { Id = 2, Header = "��������� 2", UserId = 32 },
                new Category { Id = 3, Header = "��������� 3", UserId = 32 },
                new Category { Id = 4, Header = "��������� 4", UserId = 32 }
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

                // ��������: ����� ������ GetCategoriesAsync
                var resultCategories = await categoriesDataAccess.GetCategoriesAsync(32);

                // �������� �����������
                Assert.That(resultCategories.Count, Is.EqualTo(4));
                Assert.That(resultCategories.Any(c => c.Header == "��������� 1"), Is.True);
                Assert.That(resultCategories.Any(c => c.Header == "��������� 2"), Is.True);
                Assert.That(resultCategories.Any(c => c.Header == "��������� 3"), Is.True);
                Assert.That(resultCategories.Any(c => c.Header == "��������� 4"), Is.True);
            }
        }
    }
}
