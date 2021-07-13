namespace NotifyMe.Rawbot.BestBuy.Commands
{
    using NotifyMe.Core.Mediator;
    using NotifyMe.Rawbot.BestBuy.Settings;

    public class BestBuyRequestBase<TResponse> : RequestBase<BestBuyAutomationSettings, BestBuyProductSettings, TResponse>
        where TResponse : ResponseBase
    {
    }
}