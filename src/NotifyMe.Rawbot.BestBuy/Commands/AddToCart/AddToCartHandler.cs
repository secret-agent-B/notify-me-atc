namespace NotifyMe.Rawbot.BestBuy.Commands.AddToCart
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NotifyMe.Core.Extensions;
    using NotifyMe.Rawbot.BestBuy.Commands;
    using OpenQA.Selenium;
    using Serilog;

    public class AddToCartHandler : BestBuyHandlerBase<AddToCartRequest, AddToCartResponse>
    {
        private const string _blue1 = "rgba(0, 70, 190, 1)";

        private const string _blue2 = "rgba(0, 30, 115, 1)";

        private const string _gray = "rgba(197, 203, 213, 1)";

        private const string _white = "rgba(255, 255, 255, 1)";

        private const string _yellow = "rgba(255, 224, 0, 1)";

        public AddToCartHandler(ILogger logger)
            : base(logger)
        {
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            IWebElement goToCart;

            var waitForButtonTask = await Task.Factory.StartNew(async () =>
            {
                do
                {
                    try
                    {
                        var button = this.Request.WebDriver.FindElement(SelectorType.CssSelector, ".btn-primary.add-to-cart-button", 1);

                        if (button != null)
                        {
                            var backgroundColor = button.GetCssValue("background-color").ToLower();

                            var isActive = backgroundColor == _blue1
                                           || backgroundColor == _blue2
                                           || backgroundColor == _yellow;

                            if (isActive)
                            {
                                this.Request.WebDriver.Click(SelectorType.CssSelector, ".btn-primary.add-to-cart-button", 5);

                                goToCart = this.Request.WebDriver.FindElement(SelectorType.CssSelector, ".go-to-cart-button", 5);

                                if (goToCart != null)
                                {
                                    goToCart = this.Request.WebDriver.Click(SelectorType.CssSelector, ".go-to-cart-button", 3);
                                    break;
                                }
                                else
                                {
                                    await this.Delay(5, cancellationToken);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex, "Failed to add to cart.");

                        return false;
                    }

                    await this.Delay(.1, cancellationToken);
                } while (true);

                return true;
            });

            await waitForButtonTask;
        }
    }
}