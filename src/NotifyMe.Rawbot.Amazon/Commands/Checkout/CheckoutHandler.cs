namespace NotifyMe.Rawbot.Amazon.Commands.Checkout
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using NotifyMe.Core.Contracts;
    using NotifyMe.Core.Extensions;
    using Serilog;

    public class CheckoutHandler : AmazonHandlerBase<CheckoutRequest, CheckoutResponse>
    {
        private readonly List<string> _addressUrls;
        private readonly List<string> _oosUrls;
        private readonly List<string> _paymentUrls;
        private readonly List<string> _placeOrderUrls;
        private readonly ISMSManager _smsManager;

        public CheckoutHandler(ILogger logger, ISMSManager smsManager)
            : base(logger)
        {
            this._smsManager = smsManager;

            this._addressUrls = new List<string>
            {
                "https://www.amazon.com/gp/buy/itemselect/handlers/display.html",
                "https://www.amazon.com/gp/buy/addressselect/handlers/display.html"
            };

            this._paymentUrls = new List<string>
            {
                "https://www.amazon.com/gp/buy/payselect/handlers/display.html"
            };

            this._placeOrderUrls = new List<string>
            {
                "https://www.amazon.com/gp/buy/spc/handlers/display.html"
            };

            this._oosUrls = new List<string>
            {
                "https://www.amazon.com/gp/buy/signin/handlers/continue.html"
            };
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            var retries = 0;
            var url = string.Empty;

            try
            {
                do
                {
                    url = this.Request.WebDriver.Url;

                    if (this._addressUrls.Any(x => url.StartsWith(x)))
                    {
                        this.SetAddress();
                    }
                    else if (this._paymentUrls.Any(x => url.StartsWith(x)))
                    {
                        this.SetPayment();
                    }
                    else if (this._placeOrderUrls.Any(x => url.StartsWith(x)))
                    {
                        this.PlaceOrder();
                        break;
                    }
                    else if (this._oosUrls.Any(x => url.StartsWith(x)))
                    {
                        this.TakeScreenshot("item-oos");
                        throw new Exception("item-oos");
                    }
                    else
                    {
                        if (retries < 5)
                        {
                            retries += 1;
                            await this.Delay(1, cancellationToken);
                            continue;
                        }

                        this.TakeScreenshot("unknown-location");

                        this.Logger
                            .ForContext(nameof(url), url)
                            .Error("Unknown location: {url}", url);

                        throw new Exception($"Unknown location.");
                    }
                } while (true);
            }
            catch
            {
                this.TakeScreenshot("failed-to-checkout");

                this.Logger
                    .ForContext(nameof(url), url)
                    .Error("Failed to checkout... taking a break");

                await this.Delay(5 * 60, cancellationToken);
            }
        }

        private void PlaceOrder()
        {
            if (!this.Request.AutomationSettings.IsTest)
            {
                var placeOrder = this.Request.WebDriver.Click(SelectorType.CssSelector, "#orderSummaryPrimaryActionBtn input", 3);
                this.TakeScreenshot("click-place-order");

                if (placeOrder == null)
                {
                    this.TakeScreenshot("failed-place-order");
                    throw new Exception("Failed to place order.");
                }
                else
                {
                    this._smsManager.SendMessage($"Successfully bought item {this.Request.ProductSettings.Name}", this.Request.AutomationSettings.UserAccount.PhoneNumber);
                }
            }

            this.TakeScreenshot("successfully-placed-order");
            this.Logger.Information("Successfully bought item {@productName}", this.Request.ProductSettings.Name);
        }

        private void SetAddress()
        {
            if (this.Request.WebDriver.FindElement(SelectorType.CssSelector, "#orderSummaryPrimaryActionBtn input", 3) != null)
            {
                this.TakeScreenshot("setting-address");
                this.Request.WebDriver.Click(SelectorType.CssSelector, "#orderSummaryPrimaryActionBtn input", 3);
            }
            else
            {
                this.TakeScreenshot("failed-to-set-address");
                throw new Exception("Failed to set address.");
            }
        }

        private void SetPayment()
        {
            if (this.Request.WebDriver.FindElement(SelectorType.CssSelector, "#orderSummaryPrimaryActionBtn input", 3) != null)
            {
                this.TakeScreenshot("setting-payment");
                this.Request.WebDriver.Click(SelectorType.CssSelector, "#orderSummaryPrimaryActionBtn input", 3);
            }
            else
            {
                this.TakeScreenshot("failed-to-set-payment");
                throw new Exception("Failed to set payment.");
            }
        }
    }
}