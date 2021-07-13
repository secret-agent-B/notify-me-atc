namespace NotifyMe.Rawbot.Amazon.Commands.StartCheckout
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NotifyMe.Core.Extensions;
    using Serilog;

    public class StartCheckoutHandler : AmazonHandlerBase<StartCheckoutRequest, StartCheckoutResponse>
    {
        public StartCheckoutHandler(ILogger logger)
            : base(logger)
        {
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            // click "Proceed checkout" from the product page.
            var checkout = this.Request.WebDriver.Click(SelectorType.Id, "attach-sidesheet-checkout-button", 3);

            this.TakeScreenshot("clicked-proceed-checkout");

            if (checkout == null)
            {
                this.TakeScreenshot("failed-to-start-checkout");
                throw new Exception("Failed to start check out.");
            }

            var retryCount = 0;
            var maxRetryCount = 1;

            do
            {
                var error = this.Request.WebDriver.FindElement(SelectorType.CssSelector, "b.h1", 3);

                if (error?.Text == "Oops! We're sorry")
                {
                    this.Request.WebDriver.Navigate().Refresh();
                }
                else
                {
                    return;
                }

                retryCount += 1;

                await this.Delay(.5, cancellationToken);
            } while (retryCount >= maxRetryCount);

            this.TakeScreenshot("stuck-in-were-sorry-page");
            throw new Exception("Stuck in Oops! We're sorry");
        }
    }
}