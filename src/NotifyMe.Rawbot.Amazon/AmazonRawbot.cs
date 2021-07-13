namespace NotifyMe.Rawbot.Amazon
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Options;
    using NotifyMe.Core;
    using NotifyMe.Core.Extensions;
    using NotifyMe.Core.Factories;
    using NotifyMe.Core.Mediator;
    using NotifyMe.Rawbot.Amazon.Commands;
    using NotifyMe.Rawbot.Amazon.Commands.Checkout;
    using NotifyMe.Rawbot.Amazon.Commands.ExecuteSignIn;
    using NotifyMe.Rawbot.Amazon.Commands.StartCheckout;
    using NotifyMe.Rawbot.Amazon.Commands.WaitStock;
    using NotifyMe.Rawbot.Amazon.Settings;
    using OpenQA.Selenium;
    using Serilog;

    public class AmazonRawbot : RawBotBase<AmazonAutomationSettings, AmazonProductSettings>
    {
        private const string _signOutUrl = "https://www.amazon.com/gp/flex/sign-out.html?path=%2Fgp%2Fyourstore%2Fhome&signIn=1&useRedirectOnSuccess=1&action=sign-out&ref_=nav_AccountFlyout_signout";

        private readonly IWebDriver _webDriver;

        public AmazonRawbot(
            ILogger logger,
            IMediator mediator,
            IOptions<AmazonAutomationSettings> automationSettings,
            IWebDriverFactory webDriverFactory)
            : base(logger, mediator, automationSettings.Value)
        {
            this._webDriver = webDriverFactory.Build();
        }

        public override async Task Start(AmazonProductSettings productSettings, CancellationToken cancellationToken)
        {
            var main = this._webDriver.CurrentWindowHandle;
            var isLoggedIn = false;

            do
            {
                try
                {
                    if (isLoggedIn)
                    {
                        // sign out...
                        this._webDriver.OpenNewTab(_signOutUrl, true);
                        this._webDriver.SwitchToTab(main, closeExisting: true);

                        isLoggedIn = false;
                    }

                    await this.Mediator.Send(this.BuildRequest<WaitStockRequest, WaitStockResponse>(productSettings), cancellationToken);
                    await this.Mediator.Send(this.BuildRequest<ExecuteSignInRequest, ExecuteSignInResponse>(productSettings), cancellationToken);
                    await this.Mediator.Send(this.BuildRequest<StartCheckoutRequest, StartCheckoutResponse>(productSettings), cancellationToken);
                    await this.Mediator.Send(this.BuildRequest<CheckoutRequest, CheckoutResponse>(productSettings), cancellationToken);

                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }
                catch (Exception ex)
                {
                    this.Logger.Error($"Error occured with message: ({this.SessionId}) {ex}", ex);

                    this.Logger.Warning("Cleaning up tabs for {productName}.", productSettings.Name);

                    // clean up tabs.
                    for (int i = 1; i < this._webDriver.WindowHandles.Count; i++)
                    {
                        this._webDriver.CloseTab(this._webDriver.WindowHandles[i]);
                    }
                }
            } while (true);
        }

        public override Task Stop(CancellationToken cancellationToken)
        {
            this._webDriver.Quit();

            return Task.CompletedTask;
        }

        private TRequest BuildRequest<TRequest, TResponse>(AmazonProductSettings productSettings)
            where TRequest : AmazonRequestBase<TResponse>
            where TResponse : ResponseBase
        {
            var request = Activator.CreateInstance<TRequest>();

            request.SessionId = this.SessionId;
            request.AutomationSettings = this.AutomationSettings;
            request.ProductSettings = productSettings;
            request.WebDriver = this._webDriver;

            return request;
        }
    }
}