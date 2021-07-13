namespace NotifyMe.Core.Contracts
{
    public interface IProductSettings
    {
        int BotCount { get; set; }

        public double MaxPrice { get; set; }

        public double MinPrice { get; set; }

        public string Name { get; set; }
    }
}