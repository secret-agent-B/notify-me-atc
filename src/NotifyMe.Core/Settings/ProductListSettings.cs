namespace NotifyMe.Core.Settings
{
    using System.Collections.Generic;
    using NotifyMe.Core.Contracts;

    public class ProductListSettings<TProductSettings> : IProductListSettings<TProductSettings>
        where TProductSettings : IProductSettings
    {
        public ProductListSettings()
        {
            this.Products = new List<TProductSettings>();
        }

        public List<TProductSettings> Products { get; set; }
    }
}