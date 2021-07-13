namespace NotifyMe.Web
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class Program
    {
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                WebHost
                    .CreateDefaultBuilder(args)
                    .UseSerilog()
                    .ConfigureAppConfiguration(configBuilder =>
                    {
                        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
                        {
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
                        }

                        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                        var settings = Environment.GetEnvironmentVariable("NOTIFY_ME_STORE") ?? string.Empty;

                        Log.Information("ASPNETCORE_ENVIRONMENT: {env}", env);
                        Log.Information("NOTIFY_ME_STORE: {env}", settings);

                        configBuilder
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddEnvironmentVariables()
                            .AddJsonFile("appsettings.jsonc")
                            .AddJsonFile("appsettings.UserAccount.jsonc")
                            .AddJsonFile($"appsettings.{env}.jsonc", true)
                            .AddJsonFile($"appsettings.{env}.{settings}.jsonc", true);
                    })
                    .ConfigureLogging((context, logBuilder) =>
                    {
                        Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(context.Configuration)
                            .CreateLogger()
                            .ForContext("environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                    })
                    .UseStartup<Startup>();

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
    }
}