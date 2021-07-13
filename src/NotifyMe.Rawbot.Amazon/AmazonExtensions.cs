namespace NotifyMe.Rawbot.Amazon
{
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NotifyMe.Core.Contracts;
    using NotifyMe.Rawbot.Amazon.Settings;

    public static class AmazonExtensions
    {
        public static IServiceCollection AddAmazonRawBot(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

            services.AddMediatR(typeof(AmazonExtensions).Assembly);
            services.AddTransient<IRawBot<AmazonAutomationSettings, AmazonProductSettings>, AmazonRawbot>();
            services.Configure<AmazonAutomationSettings>(configuration.GetSection($"{AmazonAutomationSettings.SectionName}"));
            services.Configure<AmazonProductListSettings>(configuration.GetSection($"{AmazonAutomationSettings.SectionName}"));

            services.AddHostedService<AmazonRawbotHost>();

            return services;
        }
    }
}