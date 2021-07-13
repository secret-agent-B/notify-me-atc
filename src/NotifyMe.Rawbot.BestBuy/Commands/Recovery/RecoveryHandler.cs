namespace NotifyMe.Rawbot.BestBuy.Commands.Recovery
{
    using NotifyMe.Core.Extensions;
    using Serilog;
    using System.Threading;
    using System.Threading.Tasks;

    public class RecoveryHandler : BestBuyHandlerBase<RecoveryRequest, RecoveryResponse>
    {
        public RecoveryHandler(ILogger logger)
            : base(logger)
        {
        }

        public override Task Execute(CancellationToken cancellation)
        {
            var phone = this.Request.AutomationSettings.UserAccount.PhoneNumber;
            var last4 = string.Empty;

            if (phone.Length >= 4)
            {
                last4 = phone.Substring(phone.Length - 4);

                if (!int.TryParse(last4, out _))
                {
                    // not a valid phone number...
                    last4 = string.Empty;
                }
            }

            if (string.IsNullOrEmpty(last4))
            {
                this.Request.WebDriver.Click(SelectorType.Id, "email-radio");
            }
            else
            {
                this.Request.WebDriver.Click(SelectorType.Id, "sms-radio");
                this.Request.WebDriver.EnterText(SelectorType.Id, "smsDigits", last4);
            }

            this.Request.WebDriver.Click(SelectorType.CssSelector, ".cia-form__controls__submit");

            return Task.CompletedTask;
        }
    }
}