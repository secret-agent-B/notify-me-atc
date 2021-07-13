namespace NotifyMe.Core.Settings
{
    public class TwilioSettings
    {
        public const string SectionName = "Twilio";

        public string AccountSID { get; set; }

        public string AuthToken { get; set; }

        public bool IsEnabled { get; set; }

        public string SenderPhoneNumber { get; set; }
    }
}