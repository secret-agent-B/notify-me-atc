namespace NotifyMe.Rawbot.BestBuy
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using NotifyMe.Core.Contracts;
    using NotifyMe.Rawbot.BestBuy.Settings;

    internal class BestBuyRawbotHost : BackgroundService
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly BestBuyProductListSettings _productList;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentBag<Task> _taskList;

        public BestBuyRawbotHost(IServiceProvider serviceProvider, IOptions<BestBuyProductListSettings> productListOpts)
        {
            this._serviceProvider = serviceProvider;
            this._productList = productListOpts.Value;
            this._taskList = new ConcurrentBag<Task>();

            this._cancellationTokenSource = new CancellationTokenSource();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var productSettings in this._productList.Products.Where(x => x.BotCount > 0))
            {
                for (int i = 0; i < productSettings.BotCount; i++)
                {
                    this._taskList.Add(Task.Factory.StartNew(async () =>
                    {
                        var rawBot = this._serviceProvider.GetRequiredService<IRawBot<BestBuyAutomationSettings, BestBuyProductSettings>>();
                        await rawBot.Start(productSettings, this._cancellationTokenSource.Token);
                    }));
                }
            }

            await Task.WhenAll(this._taskList.ToArray());
        }
    }
}