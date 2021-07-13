namespace NotifyMe.Core.Settings
{
    using NotifyMe.Core.Contracts;

    public class AutomationSettings : IAutomationSettings
    {
        public bool IsTest { get; set; } = true;

        public double RecoveryDelay { get; set; }

        public int RefreshDelayMax { get; set; } = 10;

        public int RefreshDelayMin { get; set; } = 5;

        public string ScreenshotPath { get; set; }

        public TwilioSettings Twilio { get; set; }
    }
}