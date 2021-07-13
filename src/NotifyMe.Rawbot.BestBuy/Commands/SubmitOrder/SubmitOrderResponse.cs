namespace NotifyMe.Rawbot.BestBuy.Commands.SubmitOrder
{
    using NotifyMe.Core.Mediator;

    public class SubmitOrderResponse : ResponseBase
    {
        public bool CompletedOrder { get; set; }
    }
}