namespace NotifyMe.Core.Extensions
{
    using OpenQA.Selenium;
    using System;

    public enum SelectorType
    {
        Id,
        CssSelector,
        ClassName,
        Name,
        XPath
    }

    public static class Selector
    {
        public static By Create(SelectorType selectorType, string selector)
        {
            return selectorType switch
            {
                SelectorType.Id => By.Id(selector),
                SelectorType.CssSelector => By.CssSelector(selector),
                SelectorType.ClassName => By.ClassName(selector),
                SelectorType.Name => By.Name(selector),
                SelectorType.XPath => By.XPath(selector),
                _ => throw new Exception($"Selector type {selectorType} is not valid."),
            };
        }
    }
}