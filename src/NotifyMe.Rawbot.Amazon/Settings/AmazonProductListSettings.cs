namespace NotifyMe.Rawbot.Amazon.Settings
{
    using System.Collections.Generic;
    using NotifyMe.Core.Contracts;

    public class AmazonProductListSettings : IProductListSettings<AmazonProductSettings>
    {
        public const string SectionName = "Automations:Amazon";

        public AmazonProductListSettings()
        {
            this.Products = new List<AmazonProductSettings>();
        }

        public List<AmazonProductSettings> Products { get; set; }
    }
}