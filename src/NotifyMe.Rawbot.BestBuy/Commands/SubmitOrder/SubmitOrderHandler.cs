namespace NotifyMe.Rawbot.BestBuy.Commands.SubmitOrder
{
    using System.Threading;
    using System.Threading.Tasks;
    using NotifyMe.Core.Contracts;
    using NotifyMe.Core.Extensions;
    using NotifyMe.Rawbot.BestBuy.Commands;
    using OpenQA.Selenium;
    using Serilog;

    public class SubmitOrderHandler : BestBuyHandlerBase<SubmitOrderRequest, SubmitOrderResponse>
    {
        private readonly ISMSManager _smsManager;

        public SubmitOrderHandler(ILogger logger, ISMSManager smsManager)
            : base(logger)
        {
            this._smsManager = smsManager;
        }

        public override Task Execute(CancellationToken cancellation)
        {
            IWebElement cvvField;

            do
            {
                cvvField = this.Request.WebDriver.FindElement(SelectorType.Id, "cvv", 1);
            } while (cvvField == null);

            var cvv = this.Request.WebDriver.EnterText(SelectorType.Id, "cvv", this.Request.AutomationSettings.UserAccount.CVV, 5);

            if (this.Request.AutomationSettings.IsTest)
            {
                this.Response.CompletedOrder = true;
            }
            else
            {
                var placeOrder = this.Request.WebDriver.Click(SelectorType.CssSelector, ".button--place-order-fast-track .button__fast-track");

                if (cvv != null && placeOrder != null)
                {
                    this.TakeScreenshot("order-submitted");
                    this._smsManager.SendMessage($"Successfully bought item {this.Request.ProductSettings.Name}", this.Request.AutomationSettings.UserAccount.PhoneNumber);

                    this.Response.CompletedOrder = true;
                }
            }

            return Task.CompletedTask;
        }
    }
}