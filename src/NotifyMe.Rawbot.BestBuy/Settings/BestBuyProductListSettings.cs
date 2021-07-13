namespace NotifyMe.Rawbot.BestBuy.Settings
{
    using System.Collections.Generic;
    using NotifyMe.Core.Contracts;

    public class BestBuyProductListSettings : IProductListSettings<BestBuyProductSettings>
    {
        public const string SectionName = "Automations:BestBuy";

        public BestBuyProductListSettings()
        {
            this.Products = new List<BestBuyProductSettings>();
        }

        public List<BestBuyProductSettings> Products { get; set; }
    }
}