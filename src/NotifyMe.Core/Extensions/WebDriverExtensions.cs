namespace NotifyMe.Core.Extensions
{
    using System.Linq;
    using OpenQA.Selenium;

    public static class WebDriverExtensions
    {
        public static void CloseTab(this IWebDriver webDriver, string windowHandle)
        {
            webDriver.SwitchToTab(windowHandle);
            webDriver.Close();
        }

        public static IWebDriver DisableAnimation(this IWebDriver webDriver)
        {
            const string disableScript =
                @"
                const styleElement = document.createElement('style');
                styleElement.setAttribute('id', 'style-tag');

                const styleTagCSSes = document.createTextNode('*,:after,:before{-webkit-transition:none!important;-moz-transition:none!important;-ms-transition:none!important;-o-transition:none!important;transition:none!important;-webkit-transform:none!important;-moz-transform:none!important;-ms-transform:none!important;-o-transform:none!important;transform:none!important}');
                styleElement.appendChild(styleTagCSSes);

                document.head.appendChild(styleElement);
                return true;
                ";

            webDriver.ExecuteScript<bool>(disableScript);
            return webDriver;
        }

        public static IWebDriver EnableAnimation(this IWebDriver webDriver)
        {
            const string enableScript =
                @"
                document.getElementById('style-tag').remove();
                return true;
                ";

            webDriver.ExecuteScript<bool>(enableScript);
            return webDriver;
        }

        public static TResult ExecuteScript<TResult>(this IWebDriver webDriver, string script)
        {
            var jsExecutor = (IJavaScriptExecutor)webDriver;

            return (TResult)jsExecutor.ExecuteScript(script);
        }

        public static string OpenNewTab(this IWebDriver webDriver, string url, bool switchToNewTab = true)
        {
            var curentTabCount = webDriver.WindowHandles.Count;

            _ = ExecuteScript<bool>(webDriver, $"window.open('{url}'); return true;");

            var newTabCount = webDriver.WindowHandles.Count;

            var newTabHandle = newTabCount > curentTabCount
                ? webDriver.WindowHandles.Last()
                : string.Empty;

            if (webDriver.CurrentWindowHandle != newTabHandle && !string.IsNullOrEmpty(newTabHandle) && switchToNewTab)
            {
                webDriver.SwitchToTab(newTabHandle, closeExisting: false);
            }

            return newTabHandle;
        }

        public static string SwitchToTab(this IWebDriver webDriver, string windowHandle, bool closeExisting = false)
        {
            if (closeExisting)
            {
                webDriver.Close();
            }

            webDriver.SwitchTo().Window(windowHandle);

            return windowHandle;
        }
    }
}