namespace NotifyMe.Core.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NotifyMe.Core.Contracts;
    using NotifyMe.Core.Settings;

    public static class NotificationExtensions
    {
        public static IServiceCollection AddTwilio(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

            services.Configure<TwilioSettings>(configuration.GetSection(TwilioSettings.SectionName));
            services.AddSingleton<ISMSManager, TwilioSMSManager>();

            return services;
        }
    }
}