namespace NotifyMe.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using NotifyMe.Core.Contracts;
    using Serilog;

    public abstract class RawBotBase<TAutomationSettings, TProductSettings> : IRawBot<TAutomationSettings, TProductSettings>
        where TAutomationSettings : IAutomationSettings
        where TProductSettings : IProductSettings
    {
        protected RawBotBase(ILogger logger, IMediator mediator, TAutomationSettings automationSettings)
        {
            this.Logger = logger;
            this.Mediator = mediator;
            this.AutomationSettings = automationSettings;

            this.SessionId = Guid.NewGuid();
        }

        public TAutomationSettings AutomationSettings { get; }

        public ILogger Logger { get; }

        public IMediator Mediator { get; }

        public Guid SessionId { get; set; }

        public abstract Task Start(TProductSettings productSettings, CancellationToken cancellationToken);

        public abstract Task Stop(CancellationToken cancellationToken);
    }
}