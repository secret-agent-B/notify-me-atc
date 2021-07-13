namespace NotifyMe.Core.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NotifyMe.Core.Factories;
    using NotifyMe.Core.Settings;

    public static class BrowserExtensions
    {
        public static IServiceCollection AddChrome(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

            services.AddSingleton<IWebDriverFactory, ChromeWebDriverFactory>();
            services.Configure<WebDriverSettings>(configuration.GetSection($"{WebDriverSettings.SectionName}:Chrome"));

            return services;
        }

        public static IServiceCollection AddEdge(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

            services.AddSingleton<IWebDriverFactory, EdgeWebDriverFactory>();
            services.Configure<WebDriverSettings>(configuration.GetSection($"{WebDriverSettings.SectionName}:Edge"));

            return services;
        }
    }
}