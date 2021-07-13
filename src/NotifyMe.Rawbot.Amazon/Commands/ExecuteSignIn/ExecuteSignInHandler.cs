namespace NotifyMe.Rawbot.Amazon.Commands.ExecuteSignIn
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using NotifyMe.Core.Extensions;
    using Serilog;

    public class ExecuteSignInHandler : AmazonHandlerBase<ExecuteSignInRequest, ExecuteSignInResponse>
    {
        public ExecuteSignInHandler(ILogger logger)
            : base(logger)
        {
        }

        public override Task Execute(CancellationToken cancellation)
        {
            this.Request.WebDriver.SwitchToTab(this.Request.WebDriver.WindowHandles.Last());
            var password = this.Request.WebDriver.FindElement(SelectorType.Id, "ap_password", 1);

            if (password != null)
            {
                this.TakeScreenshot("entering-password");
                this.Request.WebDriver.EnterText(SelectorType.Id, "ap_password", this.Request.AutomationSettings.UserAccount.Password);
                this.Request.WebDriver.Click(SelectorType.Id, "signInSubmit");
            }

            this.TakeScreenshot("signing-in");

            this.Request.WebDriver.SwitchToTab(this.Request.WebDriver.WindowHandles.First());

            return Task.CompletedTask;
        }
    }
}