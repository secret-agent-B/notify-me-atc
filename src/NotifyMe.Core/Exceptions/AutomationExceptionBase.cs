namespace NotifyMe.Core.Exceptions
{
    using System;

    public abstract class AutomationExceptionBase : Exception
    {
        protected AutomationExceptionBase(string message)
            : base(message)
        {
        }
    }
}