namespace NotifyMe.Rawbot.Amazon.Settings
{
    using NotifyMe.Core.Contracts;

    public class AmazonProductSettings : IProductSettings
    {
        public string ASIN { get; set; }

        public int BotCount { get; set; } = 1;

        public double MaxPrice { get; set; }

        public double MinPrice { get; set; }

        public string Name { get; set; }

        public string OfferListingId { get; set; }

        public int Quantity { get; set; }
    }
}