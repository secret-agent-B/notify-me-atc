namespace NotifyMe.Rawbot.BestBuy.Commands.SignIn
{
    using System.Threading;
    using System.Threading.Tasks;
    using NotifyMe.Core.Extensions;
    using NotifyMe.Rawbot.BestBuy.Commands;
    using Serilog;

    public class SignInHandler : BestBuyHandlerBase<SignInRequest, SignInResponse>
    {
        public SignInHandler(ILogger logger)
            : base(logger)
        {
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            this.TakeScreenshot("logging-in");

            this.Request.WebDriver.EnterText(SelectorType.Id, "fld-e", this.Request.AutomationSettings.UserAccount.Username, 1);
            this.Request.WebDriver.EnterText(SelectorType.Id, "fld-p1", this.Request.AutomationSettings.UserAccount.Password, 1);
            this.Request.WebDriver.Click(SelectorType.CssSelector, ".cia-form__controls__submit");

            await this.Delay(2, cancellationToken);
        }
    }
}