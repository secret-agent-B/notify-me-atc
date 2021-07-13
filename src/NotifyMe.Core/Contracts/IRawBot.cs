namespace NotifyMe.Core.Contracts
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Serilog;

    public interface IRawBot<TAutomationSettings, TProductSettings>
       where TAutomationSettings : IAutomationSettings
       where TProductSettings : IProductSettings
    {
        TAutomationSettings AutomationSettings { get; }

        ILogger Logger { get; }

        IMediator Mediator { get; }

        Guid SessionId { get; set; }

        Task Start(TProductSettings productSettings, CancellationToken cancellationToken);

        Task Stop(CancellationToken cancellationToken);
    }
}