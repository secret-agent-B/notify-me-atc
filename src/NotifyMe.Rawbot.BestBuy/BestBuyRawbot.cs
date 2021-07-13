namespace NotifyMe.Rawbot.BestBuy
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
    using NotifyMe.Rawbot.BestBuy.Commands;
    using NotifyMe.Rawbot.BestBuy.Commands.AddToCart;
    using NotifyMe.Rawbot.BestBuy.Commands.Checkout;
    using NotifyMe.Rawbot.BestBuy.Commands.SignIn;
    using NotifyMe.Rawbot.BestBuy.Commands.SubmitOrder;
    using NotifyMe.Rawbot.BestBuy.Commands.WaitStock;
    using NotifyMe.Rawbot.BestBuy.Settings;
    using OpenQA.Selenium;
    using Serilog;

    public class BestBuyRawbot : RawBotBase<BestBuyAutomationSettings, BestBuyProductSettings>
    {
        private const string _addToCart = "addToCart";
        private const string _checkout = "checkout";
        private const string _recovery = "recovery";
        private const string _signIn = "signIn";
        private const string _submitOrder = "submitOrder";
        private const string _verifyAccount = "verifyAccount";
        private readonly IWebDriver _webDriver;

        public BestBuyRawbot(
            ILogger logger,
            IMediator mediator,
            IOptions<BestBuyAutomationSettings> automationSettings,
            IWebDriverFactory webDriverFactory)
            : base(logger, mediator, automationSettings.Value)
        {
            this._webDriver = webDriverFactory.Build();
        }

        public override async Task Start(BestBuyProductSettings productSettings, CancellationToken cancellationToken)
        {
            try
            {
                await this.Mediator.Send(this.BuildRequest<WaitStockRequest, WaitStockResponse>(productSettings), cancellationToken);

                var automationCompleted = false;

                do
                {
                    var requestType = this.GetRequestType(productSettings);

                    switch (requestType)
                    {
                        case _addToCart:
                            await this.Mediator.Send(this.BuildRequest<AddToCartRequest, AddToCartResponse>(productSettings), cancellationToken);
                            break;

                        case _checkout:
                            await this.Mediator.Send(this.BuildRequest<StartCheckoutRequest, StartCheckoutResponse>(productSettings), cancellationToken);
                            break;

                        case _signIn:
                            await this.Mediator.Send(this.BuildRequest<SignInRequest, SignInResponse>(productSettings), cancellationToken);
                            break;

                        case _submitOrder:
                            var submitOrderResponse = await this.Mediator.Send(this.BuildRequest<SubmitOrderRequest, SubmitOrderResponse>(productSettings), cancellationToken);
                            automationCompleted = true;
                            break;

                        case _verifyAccount:
                            // when you get to this part, you're all on your own to complete your checkout.
                            automationCompleted = true;
                            break;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                } while (!automationCompleted);

                if (OperatingSystem.IsWindows())
                {
                    Console.Beep(500, 3000);
                }

                do
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                } while (this._webDriver.Url != "https://www.bestbuy.com/");

                this._webDriver.Click(SelectorType.CssSelector, ".account-button", 1);
                this._webDriver.Click(SelectorType.CssSelector, ".account-menu-logout-button", 1);
            }
            catch (NoSuchWindowException ex)
            {
                this.Logger.Error(ex, ex.Message);
                return;
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, ex.Message);
            }

            await this.Start(productSettings, cancellationToken);
        }

        public override Task Stop(CancellationToken cancellationToken)
        {
            this._webDriver.Quit();
            return Task.CompletedTask;
        }

        private TRequest BuildRequest<TRequest, TResponse>(BestBuyProductSettings productSettings)
            where TRequest : BestBuyRequestBase<TResponse>
            where TResponse : ResponseBase
        {
            var request = Activator.CreateInstance<TRequest>();

            request.SessionId = this.SessionId;
            request.AutomationSettings = this.AutomationSettings;
            request.ProductSettings = productSettings;
            request.WebDriver = this._webDriver;

            return request;
        }

        private string GetRequestType(BestBuyProductSettings productSettings)
        {
            var url = this._webDriver.Url;

            if (url.StartsWith(productSettings.Url))
            {
                return _addToCart;
            }

            if (url.StartsWith(this.AutomationSettings.CartUrl))
            {
                return _checkout;
            }

            if (url.StartsWith(this.AutomationSettings.RecoveryUrl))
            {
                return _recovery;
            }

            if (url.StartsWith(this.AutomationSettings.VerifyAccountUrl) || url.StartsWith(this.AutomationSettings.TwoStepVerificationUrl))
            {
                return _verifyAccount;
            }

            if (url.StartsWith(this.AutomationSettings.SignInUrl))
            {
                return _signIn;
            }

            if (url.StartsWith(this.AutomationSettings.CheckoutUrl))
            {
                return _submitOrder;
            }

            this.Logger
                .ForContext(nameof(url), url)
                .Error("Unknown location.");

            throw new Exception("Unknown location.");
        }
    }
}