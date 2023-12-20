using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarCourseWorkTests.IntegrationTesting
{
    public class ChangeCalendarViewTests
    {
        private IWebDriver _driver;
        private string _baseUrl;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _baseUrl = "https://localhost:7206/users/login";
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }

        [Test]
        public void Login_WithIncorrectCredentials_RedirectsToUserWasNotFoundPage()
        {
            _driver.Navigate().GoToUrl("https://localhost:7206/Events?userId=32&dateTimeFrom=12%2F21%2F2023%2000%3A00%3A00&dateTimeTo=12%2F21%2F2023%2023%3A59%3A59");
            IWebElement monthButtonOnDayPage = _driver.FindElement(By.XPath("//div[@class='btn-group']//a[contains(text(),'Месяц')]"));
            monthButtonOnDayPage.Click();

            // Добавьте здесь необходимые проверки и действия для страницы "Месяц"

            // Переход на страницу "Год" через кнопку на странице "Месяц"
            IWebElement yearButtonOnMonthPage = _driver.FindElement(By.XPath("//div[@class='btn-group']//a[contains(text(),'Год')]"));
            yearButtonOnMonthPage.Click();

            // Добавьте здесь необходимые проверки и действия для страницы "Год"

            // Переход обратно на страницу "Месяц" через кнопку на странице "Год"
            IWebElement monthButtonOnYearPage = _driver.FindElement(By.XPath("//div[@class='btn-group']//a[contains(text(),'Месяц')]"));
            monthButtonOnYearPage.Click();

            // Добавьте здесь необходимые проверки и действия для страницы "Месяц"

            // Переход на страницу "День" через кнопку на странице "Год"
            IWebElement dayButtonOnYearPage = _driver.FindElement(By.XPath("//div[@class='btn-group']//a[contains(text(),'День')]"));
            dayButtonOnYearPage.Click();
        }
    }
}
