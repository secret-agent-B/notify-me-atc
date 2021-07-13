namespace NotifyMe.Rawbot.Amazon.Commands.ReadySignIn
{
    using System.Threading;
    using System.Threading.Tasks;
    using NotifyMe.Core.Extensions;
    using Serilog;

    public class ReadySignInHandler : AmazonHandlerBase<ReadySignInRequest, ReadySignInResponse>
    {
        private const string _signInUrl = "https://www.amazon.com/ap/signin?openid.pape.max_auth_age=0&openid.return_to=https%3A%2F%2Fwww.amazon.com%2Fref%3Dnav_signin&openid.identity=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0%2Fidentifier_select&openid.assoc_handle=usflex&openid.mode=checkid_setup&openid.claimed_id=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0%2Fidentifier_select&openid.ns=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0&";

        public ReadySignInHandler(ILogger logger)
            : base(logger)
        {
        }

        public override Task Execute(CancellationToken cancellation)
        {
            var currentTab = this.Request.WebDriver.CurrentWindowHandle;
            _ = this.Request.WebDriver.OpenNewTab(_signInUrl);

            var email = this.Request.WebDriver.FindElement(SelectorType.Id, "ap_email", 1);

            if (email != null)
            {
                this.TakeScreenshot("entering-username");
                this.Request.WebDriver.EnterText(SelectorType.Id, "ap_email", this.Request.AutomationSettings.UserAccount.Username);
                this.Request.WebDriver.Click(SelectorType.Id, "continue");
            }

            this.Request.WebDriver.SwitchToTab(currentTab);

            return Task.CompletedTask;
        }
    }
}