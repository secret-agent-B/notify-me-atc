namespace NotifyMe.Core.Contracts
{
    public interface ISMSManager
    {
        string SendMessage(string message, string phoneNumber);
    }
}