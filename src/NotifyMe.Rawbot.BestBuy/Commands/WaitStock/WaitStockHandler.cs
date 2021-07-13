namespace NotifyMe.Rawbot.BestBuy.Commands.WaitStock
{
    using System.Threading;
    using System.Threading.Tasks;
    using NotifyMe.Core.Extensions;
    using NotifyMe.Rawbot.BestBuy.Commands;
    using Serilog;

    internal class WaitStockHandler : BestBuyHandlerBase<WaitStockRequest, WaitStockResponse>
    {
        public WaitStockHandler(ILogger logger)
            : base(logger)
        {
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            this.Request.WebDriver.Navigate().GoToUrl(this.Request.ProductSettings.Url);

            do
            {
                if (this.Request.WebDriver.FindElement(SelectorType.CssSelector, ".btn-primary.add-to-cart-button", 1) != null)
                {
                    this.TakeScreenshot("item-available");
                    break;
                }

                await this.DelayUsingSettings(cancellationToken);

                this.Request.WebDriver.Navigate().Refresh();
            } while (true);
        }
    }
}