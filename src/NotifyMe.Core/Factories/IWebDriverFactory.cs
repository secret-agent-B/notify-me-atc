namespace NotifyMe.Core.Factories
{
    using OpenQA.Selenium;

    public interface IWebDriverFactory
    {
        IWebDriver Build();
    }
}