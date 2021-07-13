namespace NotifyMe.Rawbot.Amazon.Settings
{
    using NotifyMe.Core.Settings;

    public class AmazonAutomationSettings : AutomationSettings
    {
        public const string SectionName = "Automations:Amazon";

        public AmazonAutomationSettings()
        {
            this.UserAccount = new AmazonUserAccountSettings();
        }

        public int CheckLimit { get; set; }

        public AmazonUserAccountSettings UserAccount { get; set; }
    }
}