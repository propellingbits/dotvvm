﻿using Riganti.Utils.Testing.Selenium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotVVM.Samples.Tests.Control
{
    [TestClass]
    public class LiteralTests : AppSeleniumTest
    {

        [TestMethod]
        public void Control_Literal_Literal()
        {
            //TODO Rewrite to new AssertApi
            throw new NotImplementedException();
            //RunInAllBrowsers(browser =>
            //{
            //    browser.NavigateToUrl(SamplesRouteUrls.ControlSamples_Literal_Literal);

            //    foreach (var column in browser.FindElements("td"))
            //    {
            //        column.ElementAt("fieldset", 0).Single("span").Check().InnerText(s=> s.Equals("Hardcoded value"));
            //        column.ElementAt("fieldset", 1).Single("span").Check().InnerText(s => s.Equals("Hello"));
            //        column.ElementAt("fieldset", 2).Single("span").Check().InnerText(s => s.Equals("1/1/2000"));

            //        column.ElementAt("fieldset", 3).FindElements("span").ThrowIfDifferentCountThan(0);
            //        column.ElementAt("fieldset", 3).Check().InnerText(t => t.Contains("Hardcoded value"));

            //        column.ElementAt("fieldset", 4).FindElements("span").ThrowIfDifferentCountThan(0);
            //        column.ElementAt("fieldset", 4).Check().InnerText(t => t.Contains("Hello"));

            //        column.ElementAt("fieldset", 5).FindElements("span").ThrowIfDifferentCountThan(0);
            //        column.ElementAt("fieldset", 5).Check().InnerText(t => t.Contains("1/1/2000"));
            //    }
            //});
        }
        [TestMethod]
        public void Control_Literal_Literal_FormatString()
        {
            //TODO Rewrite CheckElementWrapper in selenium api
            throw new NotImplementedException();
            //RunInAllBrowsers(browser =>
            //{
            //    Action<string> checkFormat = (string format) =>
            //    {
            //        //check format d
            //        var text = browser.First("#results-" + format).GetText();
            //        browser.First("#client-format-" + format).CheckIfInnerTextEquals(text, false);
            //        browser.First("#server-render-format-" + format).Check().InnerText(s=> s.Equals(text, StringComparison.OrdinalIgnoreCase));

            //    };
            //    browser.NavigateToUrl(SamplesRouteUrls.ControlSamples_Literal_Literal_FormatString);
            //    browser.First("#change-culture").Click();

            //    checkFormat("d");
            //    checkFormat("D");
            //    //dd
            //});
        }

        [TestMethod]
        public void Control_Literal_Literal_CollectionLength()
        {
            //TODO Rewrite CheckElementWrapper in selenium api
            throw new NotImplementedException();
            //RunInAllBrowsers(browser =>
            //{
            //    browser.NavigateToUrl(SamplesRouteUrls.ControlSamples_Literal_Literal_CollectionLength);


            //    browser.Single("span").CheckIfInnerText(s => s.Contains("0"));
            //    browser.Single("#second").CheckIfIsNotDisplayed();
            //    browser.First("#first").Click();

            //    browser.Single("span").Check().InnerText(s => s.Contains("1"));
            //    browser.Single("#second").CheckIfIsNotDisplayed();
            //    browser.First("#first").Click();


            //    browser.Single("span").Check().InnerText(s => s.Contains("2"));
            //    browser.Single("#second").CheckIfIsNotDisplayed();
            //    browser.First("#first").Click();

            //    browser.Single("span").Check().InnerText(s => s.Contains("3"));
            //    browser.Single("#second").CheckIfIsDisplayed();
            //});
        }
        [TestMethod]
        public void Control_Literal_Literal_ArrayLength()
        {
            //TODO Rewrite CheckElementWrapper in selenium api
            throw new NotImplementedException();
            //RunInAllBrowsers(browser =>
            //{
            //    browser.NavigateToUrl(SamplesRouteUrls.ControlSamples_Literal_Literal_ArrayLength);


            //    browser.Single("span").Check().InnerText(s => s.Contains("0"));
            //    browser.Single("#second").CheckIfIsNotDisplayed();
            //    browser.First("#first").Click();

            //    browser.Single("span").Check().InnerText(s => s.Contains("1"));
            //    browser.Single("#second").CheckIfIsNotDisplayed();
            //    browser.First("#first").Click();


            //    browser.Single("span").Check().InnerText(s => s.Contains("2"));
            //    browser.Single("#second").CheckIfIsNotDisplayed();
            //    browser.First("#first").Click();

            //    browser.Single("span").Check().InnerText(s => s.Contains("3"));
            //    browser.Single("#second").CheckIfIsDisplayed();
            //});
        }

    }
}