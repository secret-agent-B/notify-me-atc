namespace NotifyMe.Core.Factories
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Options;
    using NotifyMe.Core.Settings;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    public class ChromeWebDriverFactory : IWebDriverFactory
    {
        private readonly WebDriverSettings _webDriverSettings;

        public ChromeWebDriverFactory(IOptions<WebDriverSettings> options)
        {
            this._webDriverSettings = options.Value;
        }

        public IWebDriver Build()
        {
            var chromeOptions = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Eager,
                AcceptInsecureCertificates = false
            };

            foreach (var argument in this._webDriverSettings.Arguments)
            {
                chromeOptions.AddArgument(argument);
            }

            chromeOptions.AddExcludedArguments(new List<string> { "enable-automation" });

            var services = ChromeDriverService.CreateDefaultService(this._webDriverSettings.DriverPath);

            return new ChromeDriver(services, chromeOptions);
        }
    }
}