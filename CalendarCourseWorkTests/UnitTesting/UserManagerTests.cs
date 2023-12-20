using Moq;
using CalendarCourseWork.BusinessLogic.Storages;
using CalendarCourseWork.BusinessLogic.Models;
using CalendarCourseWork.Logic;

namespace CalendarCourseWorkTests
{
    [TestFixture]
    public class UsersManagerTests
    {
        [Test]
        public async Task GetUserByEmailAndPassword_ValidCredentials_ReturnsUser()
        {
            // Устанавливаем
            var mockUsersDataAccess = new Mock<UsersDataAccess>();
            var userManager = new UsersManager(mockUsersDataAccess.Object);

            // Мокируем метод GetUserByEmailAndPassword
            mockUsersDataAccess.Setup(m => m.GetUserByEmailAndPassword("7777bulat7777@gmail.com", "Password"))
                .ReturnsAsync(new User { Id = 32, Name = "Тестовый Пользователь", Email = "7777bulat7777@gmail.com", Password = "Password" });

            // Действие
            var resultUser = await userManager.GetUserByEmailAndPassword("7777bulat7777@gmail.com", "Password");

            // Проверка
            Assert.That(resultUser, Is.Not.Null);
            Assert.That(resultUser.Id, Is.EqualTo(32));
            Assert.That(resultUser.Name, Is.EqualTo("Тестовый Пользователь"));
            Assert.That(resultUser.Email, Is.EqualTo("7777bulat7777@gmail.com"));
            Assert.That(resultUser.Password, Is.EqualTo("Password"));
        }

        [Test]
        public async Task GetUserByIdAsync_ValidId_ReturnsUser()
        {
            // Устанавливаем
            var mockUsersDataAccess = new Mock<UsersDataAccess>();
            var userManager = new UsersManager(mockUsersDataAccess.Object);

            // Мокируем метод GetUserByIdAsync
            mockUsersDataAccess.Setup(m => m.GetUserByIdAsync(32))
                .ReturnsAsync(new User { Id = 32, Name = "Тестовый Пользователь", Email = "7777bulat7777@gmail.com" });

            // Действие
            var resultUser = await userManager.GetUserByIdAsync(32);

            // Проверка
            Assert.That(resultUser, Is.Not.Null);
            Assert.That(resultUser.Id, Is.EqualTo(32));
            Assert.That(resultUser.Name, Is.EqualTo("Тестовый Пользователь"));
            Assert.That(resultUser.Email, Is.EqualTo("7777bulat7777@gmail.com"));
        }

        [Test]
        public async Task GetUserByEmailAndPassword_InvalidCredentials_ReturnsNull()
        {
            // Устанавливаем
            var mockUsersDataAccess = new Mock<UsersDataAccess>();
            var userManager = new UsersManager(mockUsersDataAccess.Object);

            // Мокируем метод GetUserByEmailAndPassword для неверных учетных данных
            mockUsersDataAccess.Setup(m => m.GetUserByEmailAndPassword("invalidemail@example.com", "WrongPassword"))
                .ReturnsAsync((User)null);

            // Действие
            var resultUser = await userManager.GetUserByEmailAndPassword("invalidemail@example.com", "WrongPassword");

            // Проверка
            Assert.That(resultUser, Is.Null);
        }
    }
}
