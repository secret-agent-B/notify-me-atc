namespace NotifyMe.Rawbot.BestBuy.Commands
{
    using NotifyMe.Core.Mediator;
    using NotifyMe.Rawbot.BestBuy.Settings;
    using Serilog;

    public abstract class BestBuyHandlerBase<TRequest, TResponse> : HandlerBase<BestBuyAutomationSettings, BestBuyProductSettings, TRequest, TResponse>
        where TRequest : BestBuyRequestBase<TResponse>
        where TResponse : ResponseBase, new()
    {
        protected BestBuyHandlerBase(ILogger logger)
            : base(logger)
        {
        }
    }
}