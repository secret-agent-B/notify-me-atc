namespace NotifyMe.Rawbot.BestBuy.Settings
{
    using NotifyMe.Core.Settings;

    public class BestBuyAutomationSettings : AutomationSettings
    {
        public const string SectionName = "Automations:BestBuy";

        public BestBuyAutomationSettings()
        {
            this.UserAccount = new BestBuyUserAccountSettings();
        }

        public string CartUrl { get; set; }

        public string CheckoutUrl { get; set; }

        public string RecoveryUrl { get; set; }

        public string SignInUrl { get; set; }

        public string TwoStepVerificationUrl { get; set; }

        public BestBuyUserAccountSettings UserAccount { get; set; }

        public string VerifyAccountUrl { get; set; }
    }
}