namespace NotifyMe.Core.Contracts
{
    using NotifyMe.Core.Settings;

    public interface IAutomationSettings
    {
        bool IsTest { get; set; }

        int RefreshDelayMax { get; set; }

        int RefreshDelayMin { get; set; }

        string ScreenshotPath { get; set; }

        TwilioSettings Twilio { get; set; }
    }
}