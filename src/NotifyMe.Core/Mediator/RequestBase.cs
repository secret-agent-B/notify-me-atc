namespace NotifyMe.Core.Mediator
{
    using System;
    using MediatR;
    using NotifyMe.Core.Contracts;
    using OpenQA.Selenium;

    public abstract class RequestBase<TAutomationSettings, TProductSettings, TResponse> : IRequest<TResponse>
        where TAutomationSettings : IAutomationSettings
        where TProductSettings : IProductSettings
        where TResponse : ResponseBase
    {
        public TAutomationSettings AutomationSettings { get; set; }

        public TProductSettings ProductSettings { get; set; }

        public Guid SessionId { get; set; }

        public IWebDriver WebDriver { get; set; }
    }
}