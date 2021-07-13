namespace NotifyMe.Core.Mediator
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using NotifyMe.Core.Contracts;
    using OpenQA.Selenium;
    using Serilog;

    public abstract class HandlerBase<TAutomationSettings, TProductSettings, TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TAutomationSettings : IAutomationSettings
        where TProductSettings : IProductSettings
        where TRequest : RequestBase<TAutomationSettings, TProductSettings, TResponse>
        where TResponse : ResponseBase, new()
    {
        public HandlerBase(ILogger logger)
        {
            this.Logger = logger;
        }

        public ILogger Logger { get; }

        public TRequest Request { get; private set; }

        public TResponse Response { get; private set; }

        public async Task Delay(double delay, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(delay), cancellationToken);
        }

        public async Task DelayUsingSettings(CancellationToken cancellationToken)
        {
            var delay = new Random().Next(this.Request.AutomationSettings.RefreshDelayMin, this.Request.AutomationSettings.RefreshDelayMax);
            await Task.Delay(TimeSpan.FromSeconds(delay), cancellationToken);
        }

        public abstract Task Execute(CancellationToken cancellation);

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            this.Request = request;
            this.Response = Activator.CreateInstance<TResponse>();

            this.Logger
                .ForContext(nameof(this.Request.SessionId), this.Request.SessionId);

            await this.Execute(cancellationToken);

            return this.Response;
        }

        public string TakeScreenshot(string name = "")
        {
            var filename = string.IsNullOrWhiteSpace(name) ? this.GetType().Name : name;
            var directory = Path.Combine(this.Request.AutomationSettings.ScreenshotPath, this.Request.ProductSettings.Name, this.Request.SessionId.ToString());

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            filename = $"{DateTime.Now:MMdd_hhmmss}_{filename}.png";

            var fullpath = Path.Combine(directory, filename);

            ((ITakesScreenshot)this.Request.WebDriver)
                .GetScreenshot()
                .SaveAsFile(fullpath);

            this.Logger
                .ForContext(nameof(directory), directory)
                .ForContext(nameof(filename), filename)
                .ForContext(nameof(fullpath), fullpath)
                .ForContext(nameof(this.Request.WebDriver.Url), this.Request.WebDriver.Url)
                .Debug("Screenshot saved as {filename}.", filename);

            return fullpath;
        }
    }
}