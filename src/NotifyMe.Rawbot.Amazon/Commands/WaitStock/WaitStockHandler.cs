namespace NotifyMe.Rawbot.Amazon.Commands.WaitStock
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using NotifyMe.Core.Extensions;
    using NotifyMe.Rawbot.Amazon.Commands.ReadySignIn;
    using OpenQA.Selenium;
    using Serilog;

    public class WaitStockHandler : AmazonHandlerBase<WaitStockRequest, WaitStockResponse>
    {
        private const string _offerListingIDSelector = "offerListingID";
        private const string _tempProductUrl = "https://www.amazon.com/Syntech-Adapter-Thunderbolt-Compatible-MacBook/dp/B07CVX3516";

        private readonly IMediator _mediator;

        public WaitStockHandler(ILogger logger, IMediator mediator)
            : base(logger)
        {
            this._mediator = mediator;
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            this.InitializePage();

            await this._mediator.Send(new ReadySignInRequest
            {
                AutomationSettings = this.Request.AutomationSettings,
                ProductSettings = this.Request.ProductSettings,
                SessionId = this.Request.SessionId,
                WebDriver = this.Request.WebDriver
            }, cancellationToken);

            var loopCount = 0;

            do
            {
                this.ClickATC();

                if (await this.CheckCart(cancellationToken))
                {
                    this.TakeScreenshot("in-stock");
                    break;
                }
                else
                {
                    try
                    {
                        this.Request.WebDriver.Click(SelectorType.Id, "attach-close_sideSheet-link", 3);
                    }
                    catch (ElementClickInterceptedException ex)
                    {
                        this.TakeScreenshot("ex-element-not-visible");

                        this.Logger.Error(ex, ex.Message);
                        this.InitializePage();
                    }
                }

                loopCount++;

                if (loopCount >= this.Request.AutomationSettings.CheckLimit)
                {
                    // reset.
                    loopCount = 0;

                    // to prevent lag...
                    this.InitializePage(false);
                }

                await this.DelayUsingSettings(cancellationToken);
            } while (true);
        }

        private async Task<bool> CheckCart(CancellationToken cancellationToken)
        {
            string purchasePrice;

            do
            {
                var price = this.Request.WebDriver.FindElement(SelectorType.Id, "attach-accessory-cart-subtotal", 3);

                if (price == null)
                {
                    throw new Exception("Could not find price summary.");
                }

                purchasePrice = price.Text.Trim();

                await this.Delay(.3, cancellationToken);
            } while (string.IsNullOrEmpty(purchasePrice));

            var inStock = purchasePrice != "$0.00";

            return inStock;
        }

        private void ClickATC()
        {
            var atc = this.Request.WebDriver.Click(SelectorType.Id, "add-to-cart-button", 3);

            if (atc == null)
            {
                this.TakeScreenshot("no-atc-button");
                throw new Exception("Could not find ATC button.");
            }
        }

        private void InitializePage(bool takeScreenshot = true)
        {
            this.Request.WebDriver.SwitchToTab(this.Request.WebDriver.WindowHandles[0]);

            this.Request.WebDriver
                .Navigate()
                .GoToUrl(_tempProductUrl);

            this.Request.WebDriver.DisableAnimation();

            if (takeScreenshot)
            {
                this.TakeScreenshot("page-initialized");
            }

            var offerListingIdElement = this.Request.WebDriver.FindElement(SelectorType.Id, _offerListingIDSelector, 3);

            if (offerListingIdElement == null)
            {
                this.TakeScreenshot("offerlistingid-field-not-found");
                throw new Exception($"Failed to find element by ID ({_offerListingIDSelector}) to set the offer listing id.");
            }

            var offerListingId = this.Request.WebDriver.ExecuteScript<string>(
                            $"document.getElementById('{_offerListingIDSelector}').value = '{this.Request.ProductSettings.OfferListingId}'; return document.getElementById('{_offerListingIDSelector}').value;");

            this.Request.WebDriver.ExecuteScript<string>($"document.title = '{this.Request.ProductSettings.Name}-{this.Request.SessionId}'; return document.title;");

            if (offerListingId != this.Request.ProductSettings.OfferListingId)
            {
                this.TakeScreenshot("offerlistingid-field-not-set");
                throw new Exception($"Failed to set the offer listing id for {this.Request.ProductSettings.Name}.");
            }
        }
    }
}