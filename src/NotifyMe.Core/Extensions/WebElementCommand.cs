namespace NotifyMe.Core.Extensions
{
    using OpenQA.Selenium;

    public static class WebElementCommand
    {
        public static IWebElement Click(this IWebDriver webDriver, SelectorType selectorType, string selector, double timeout = default)
        {
            var element = webDriver.FindElement(selectorType, selector, timeout);

            element?.Click();

            return element;
        }

        public static IWebElement EnterText(this IWebDriver webDriver, SelectorType selectorType, string selector, string value, double timeout = default)
        {
            var element = webDriver.FindElement(selectorType, selector, timeout);

            element?.SendKeys(value);

            return element;
        }
    }
}