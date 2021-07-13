namespace NotifyMe.Core.Settings
{
    using System.Collections.Generic;

    public class WebDriverSettings
    {
        public static readonly string SectionName = "WebDriverSettings";

        public WebDriverSettings()
        {
            this.Arguments = new List<string>();
        }

        public IList<string> Arguments { get; set; }

        public string BrowserPath { get; set; }

        public string DriverPath { get; set; }

        public string ProfilePath { get; set; }
    }
}