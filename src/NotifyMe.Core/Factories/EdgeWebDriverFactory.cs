namespace NotifyMe.Core.Factories
{
    using System.Drawing;
    using Microsoft.Edge.SeleniumTools;
    using Microsoft.Extensions.Options;
    using NotifyMe.Core.Settings;
    using OpenQA.Selenium;

    /// <summary>
    /// Edge web driver factory.
    /// </summary>
    /// <seealso cref="IWebDriverFactory" />
    public class EdgeWebDriverFactory : IWebDriverFactory
    {
        private readonly WebDriverSettings _webDriverSettings;

        public EdgeWebDriverFactory(IOptions<WebDriverSettings> options)
        {
            this._webDriverSettings = options.Value;
        }

        public IWebDriver Build()
        {
            var edgeOptions = new EdgeOptions
            {
                UseChromium = true,
                PageLoadStrategy = PageLoadStrategy.Eager,
                AcceptInsecureCertificates = false,
            };

            foreach (var argument in this._webDriverSettings.Arguments)
            {
                edgeOptions.AddArgument(argument);
            }

            var service = EdgeDriverService.CreateChromiumService(this._webDriverSettings.DriverPath);
            var driver = new EdgeDriver(service, edgeOptions);

            driver.Manage().Window.Size = new Size(1980, 1080);

            return driver;
        }
    }
}