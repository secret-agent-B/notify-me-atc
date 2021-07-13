namespace NotifyMe.Rawbot.Amazon.Commands
{
    using NotifyMe.Core.Mediator;
    using NotifyMe.Rawbot.Amazon.Settings;
    using Serilog;

    public abstract class AmazonHandlerBase<TRequest, TResponse> : HandlerBase<AmazonAutomationSettings, AmazonProductSettings, TRequest, TResponse>
        where TRequest : AmazonRequestBase<TResponse>
        where TResponse : ResponseBase, new()
    {
        protected AmazonHandlerBase(ILogger logger)
            : base(logger)
        {
        }
    }
}