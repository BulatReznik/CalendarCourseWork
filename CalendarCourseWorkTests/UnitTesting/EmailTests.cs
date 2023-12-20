using CalendarCourseWork.BusinessLogic.Models;
using CalendarCourseWork;
using Moq;
using CalendarCourseWork.Logic;

namespace CalendarCourseWorkTests
{
    [TestFixture]
    public class EmailTests
    {
        [Test]
        public void SendNotification_SendsEmailWithCorrectParameters()
        {
            // Arrange

            var @event = new Event
            {
                EventName = "TestEvent",
                EventTime = DateTime.Now,
                EventDescription = "This is a test event."
            };

            var user = new User
            {
                Id = 32,
                Email = "7777bulat7777@gmail.com",
                Name = "Пользователь для теста",
                Password = "password",
            };

            string to = user.Email;
            string subject = "Напоминание: " + @event.EventName; // Заголовок вашего уведомления
            string html = $"Уважаемый {user.Name},<br/><br/> Напоминаем вам о событии: {@event.EventName} в {@event.EventTime}. Описание события: {@event.EventDescription}";
            string from = "7777bulat7777@gmail.com"; // Замените на свой адрес электронной почты

            var emailService = new EmailService();

            // Act
            bool emailSendSuccess = emailService.SendEmail(to, subject, html, from);

            // Assert
            Assert.That(emailSendSuccess, Is.True);
        }
    }
}