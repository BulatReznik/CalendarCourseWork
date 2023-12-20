using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CalendarCourseWorkTests
{
    [TestFixture]
    public class LoginPageTests
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
            // Arrange
            _driver.Navigate().GoToUrl(_baseUrl);

            // Act
            _driver.FindElement(By.Id("Email")).SendKeys("invalid@example.com");
            _driver.FindElement(By.Id("Password")).SendKeys("invalidpassword");
            _driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            // Assert
            string currentUrl = _driver.Url;
            Assert.That(currentUrl, Is.EqualTo("https://localhost:7206/Users/UserWas"));
        }

        [Test]
        public void Login_WithCorrectCredentials_RedirectsToCalendarPage()
        {
            // Arrange
            _driver.Navigate().GoToUrl(_baseUrl);

            // Act
            _driver.FindElement(By.Id("Email")).SendKeys("7777bulat7777@gmail.com");
            _driver.FindElement(By.Id("Password")).SendKeys("Password");
            _driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            // Assert
            string currentUrl = _driver.Url;
            Assert.That(currentUrl, Is.EqualTo("https://localhost:7206/Calendar/MonthWithEvents?year=2023&month=12&userId=32"));
        }
    }
}
