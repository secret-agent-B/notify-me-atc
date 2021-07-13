namespace NotifyMe.Rawbot.Amazon.Commands
{
    using NotifyMe.Core.Mediator;
    using NotifyMe.Rawbot.Amazon.Settings;

    public class AmazonRequestBase<TResponse> : RequestBase<AmazonAutomationSettings, AmazonProductSettings, TResponse>
        where TResponse : ResponseBase
    {
    }
}