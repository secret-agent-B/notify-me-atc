namespace NotifyMe.Core.Extensions
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using OpenQA.Selenium;

    public static class WebElementQuery
    {
        public static IWebElement FindElement(this IWebDriver webDriver, SelectorType selectorType, string selector, double timeout = default)
        {
            IWebElement element;

            if (timeout == default)
            {
                element = WaitElement(webDriver, selectorType, selector);
            }
            else
            {
                element = WaitElementWithTimeout(webDriver, selectorType, selector, timeout);
            }

            return element;
        }

        private static IWebElement WaitElement(IWebDriver webDriver, SelectorType selectorType, string selector)
        {
            IWebElement element;

            var bySelector = Selector.Create(selectorType, selector);

            do
            {
                element = webDriver.FindElements(bySelector).FirstOrDefault();

                if (element == null)
                {
                    Task.Delay(100).Wait();
                }
            } while (element == null);

            return element;
        }

        private static IWebElement WaitElementWithTimeout(IWebDriver webDriver, SelectorType selectorType, string selector, double timeout)
        {
            IWebElement element;

            var bySelector = Selector.Create(selectorType, selector);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                element = webDriver.FindElements(bySelector).FirstOrDefault();

                if (element != null)
                {
                    stopwatch.Stop();
                    break;
                }

                Task.Delay(100).Wait();
            } while (stopwatch.Elapsed.TotalSeconds < timeout);

            return element;
        }
    }
}