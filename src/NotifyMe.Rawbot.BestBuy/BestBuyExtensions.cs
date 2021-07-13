namespace NotifyMe.Rawbot.BestBuy
{
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NotifyMe.Core.Contracts;
    using NotifyMe.Rawbot.BestBuy.Settings;

    public static class BestBuyExtensions
    {
        public static IServiceCollection AddBestBuyRawBot(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

            services.AddMediatR(typeof(BestBuyExtensions).Assembly);
            services.AddTransient<IRawBot<BestBuyAutomationSettings, BestBuyProductSettings>, BestBuyRawbot>();
            services.Configure<BestBuyAutomationSettings>(configuration.GetSection($"{BestBuyAutomationSettings.SectionName}"));
            services.Configure<BestBuyProductListSettings>(configuration.GetSection($"{BestBuyAutomationSettings.SectionName}"));

            services.AddHostedService<BestBuyRawbotHost>();

            return services;
        }
    }
}