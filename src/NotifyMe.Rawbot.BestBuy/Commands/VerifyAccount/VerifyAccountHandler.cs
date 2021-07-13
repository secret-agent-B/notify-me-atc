namespace NotifyMe.Rawbot.BestBuy.Commands.VerifyAccount
{
    using System.Threading;
    using System.Threading.Tasks;
    using NotifyMe.Core.Extensions;
    using Serilog;

    public class VerifyAccountHandler : BestBuyHandlerBase<VerifyAccountRequest, VerifyAccountResponse>
    {
        public VerifyAccountHandler(ILogger logger)
            : base(logger)
        {
        }

        public override async Task Execute(CancellationToken cancellation)
        {
            do
            {
                var code = this.Request.WebDriver.ExecuteScript<string>("let code = prompt(\"Enter security code:\"); return code;");

                this.Request.WebDriver.EnterText(SelectorType.Id, "verificationCode", code);
                this.Request.WebDriver.Click(SelectorType.Id, "cia-form__controls__submit");

                await this.Delay(1, cancellation);
            } while (
                this.Request.WebDriver.Url.StartsWith(this.Request.AutomationSettings.VerifyAccountUrl)
                || this.Request.WebDriver.Url.StartsWith(this.Request.AutomationSettings.TwoStepVerificationUrl));
        }
    }
}