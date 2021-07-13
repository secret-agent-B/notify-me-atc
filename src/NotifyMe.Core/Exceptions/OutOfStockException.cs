namespace NotifyMe.Core.Exceptions
{
    public class OutOfStockException : AutomationExceptionBase
    {
        private OutOfStockException(string message)
            : base(message)
        {
        }

        public static void Throw(string message)
        {
            throw new OutOfStockException(message);
        }
    }
}