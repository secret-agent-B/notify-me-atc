namespace NotifyMe.Rawbot.BestBuy.Commands.Checkout
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using NotifyMe.Core.Extensions;
    using NotifyMe.Rawbot.BestBuy.Commands;
    using NotifyMe.Rawbot.BestBuy.Commands.SignIn;
    using Serilog;

    public class StartCheckoutHandler : BestBuyHandlerBase<StartCheckoutRequest, StartCheckoutResponse>
    {
        private readonly IMediator _mediator;

        public StartCheckoutHandler(ILogger logger, IMediator mediator)
            : base(logger)
        {
            this._mediator = mediator;
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            this.TakeScreenshot("starting-checkout");

            this.Request.WebDriver.Click(SelectorType.CssSelector, ".checkout-buttons__checkout button");

            await this.Delay(2, cancellationToken);
        }
    }
}