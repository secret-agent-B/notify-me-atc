namespace NotifyMe.Core.Contracts
{
    using System.Collections.Generic;

    public interface IProductListSettings<TProductSettings>
        where TProductSettings : IProductSettings
    {
        List<TProductSettings> Products { get; set; }
    }
}