namespace NotifyMe.Rawbot.BestBuy.Settings
{
    using System;
    using NotifyMe.Core.Contracts;

    public class BestBuyProductSettings : IProductSettings
    {
        public int BotCount { get; set; }

        public double MaxPrice { get; set; }

        public double MinPrice { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}