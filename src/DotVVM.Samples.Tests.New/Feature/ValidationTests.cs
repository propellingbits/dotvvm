﻿using System;
using System.Globalization;
using DotVVM.Testing.Abstractions;
using Riganti.Selenium.Core;
using Xunit;
using Riganti.Selenium.DotVVM;
using Xunit.Abstractions;

namespace DotVVM.Samples.Tests.New.Feature
{
    public class ValidationTests : AppSeleniumTest
    {
        [Fact]
        public void Feature_Validation_ClientSideValidationDisabling()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_ClientSideValidationDisabling);

                var requiredValidationResult = browser.Single("requiredValidationResult", SelectBy.Id);
                var emailValidationResult = browser.Single("emailValidationResult", SelectBy.Id);
                var validationTriggerButton = browser.First("input[type=button]");
                var requiredTextbox = browser.Single("required", SelectBy.Id);
                var emailTextbox = browser.Single("email", SelectBy.Id);

                AssertUI.IsNotDisplayed(requiredValidationResult);
                AssertUI.IsNotDisplayed(emailValidationResult);

                requiredTextbox.SendKeys("test");
                emailTextbox.SendKeys("test@test.test");
                validationTriggerButton.Click();

                AssertUI.IsNotDisplayed(requiredValidationResult);
                AssertUI.IsNotDisplayed(emailValidationResult);

                requiredTextbox.Clear();
                emailTextbox.Clear();
                emailTextbox.SendKeys("notEmail");
                validationTriggerButton.Click();

                AssertUI.IsDisplayed(requiredValidationResult);
                AssertUI.IsDisplayed(emailValidationResult);
            });
        }


        [Fact]
        [SampleReference(nameof(SamplesRouteUrls.FeatureSamples_Validation_ClientSideValidationDisabling))]
        public void Feature_Validation_ClientSideValidationDisabling_Enabled()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_ClientSideValidationDisabling + "/true");

                var requiredValidationResult = browser.Single("requiredValidationResult", SelectBy.Id);
                var emailValidationResult = browser.Single("emailValidationResult", SelectBy.Id);
                var validationTriggerButton = browser.First("input[type=button]");
                var requiredTextbox = browser.Single("required", SelectBy.Id);
                var emailTextbox = browser.Single("email", SelectBy.Id);

                AssertUI.IsNotDisplayed(requiredValidationResult);
                AssertUI.IsNotDisplayed(emailValidationResult);

                requiredTextbox.SendKeys("test");
                emailTextbox.SendKeys("test@test.test");
                validationTriggerButton.Click();

                AssertUI.IsNotDisplayed(requiredValidationResult);
                AssertUI.IsNotDisplayed(emailValidationResult);

                requiredTextbox.Clear();
                emailTextbox.Clear();
                emailTextbox.SendKeys("notEmail");
                validationTriggerButton.Click();

                AssertUI.IsDisplayed(requiredValidationResult);
                AssertUI.IsNotDisplayed(emailValidationResult);
            });
        }

        [Fact]
        public void Feature_Validation_DateTimeValidation()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_DateTimeValidation);

                var textBox = browser.First("input[type=text]");
                var button = browser.First("input[type=button]");

                // empty field - should have error
                textBox.Clear();
                button.Click();
                AssertUI.HasClass(textBox, "has-error");

                // corrent value - no error
                textBox.SendKeys("06/14/2017 8:10:35 AM");
                browser.Wait(2000);
                button.Click();
                AssertUI.HasNotClass(textBox, "has-error");

                // incorrent value - should have error
                textBox.Clear();
                textBox.SendKeys("06-14-2017");
                button.Click();
                AssertUI.HasClass(textBox, "has-error");

                // correct value - no error
                textBox.Clear();
                textBox.SendKeys("10/13/2017 10:30:50 PM");
                button.Click();
                AssertUI.HasNotClass(textBox, "has-error");
            });
        }

        [Fact]
        public void Feature_Validation_DateTimeValidation_NullableDateTime()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_DateTimeValidation_NullableDateTime);
                var textBox1 = browser.ElementAt("input[type=text]", 0);
                var textBox2 = browser.ElementAt("input[type=text]", 1);
                var button = browser.Single("input[type=button]");
                var errorField = browser.First(".validation-error");

                // empty field - no error
                textBox1.Clear();
                button.Click();
                AssertUI.HasNotClass(textBox1, "has-error");
                AssertUI.HasNotClass(textBox2, "has-error");
                AssertUI.IsNotDisplayed(errorField);

                // invalid value - should report error
                textBox1.SendKeys("06-14-2017");
                button.Click();
                AssertUI.HasClass(textBox1, "has-error");
                AssertUI.HasClass(textBox2, "has-error");
                AssertUI.IsDisplayed(errorField);

                // valid value - no error
                textBox1.Clear();
                textBox1.SendKeys(DateTime.Now.ToString("dd/MM/yyyy H:mm:ss", CultureInfo.InvariantCulture));
                button.Click();
                AssertUI.HasNotClass(textBox1, "has-error");
                AssertUI.HasNotClass(textBox2, "has-error");
                AssertUI.IsNotDisplayed(errorField);
                AssertUI.Value(textBox1, textBox2.GetValue());

                // one textbox has invalid value and second gets valid - should have no error
                textBox1.Clear();
                textBox1.SendKeys("Invalid value");
                textBox2.SendKeys(DateTime.Now.ToString("dd/MM/yyyy H:mm:ss", CultureInfo.InvariantCulture));
                button.Click();
                AssertUI.HasNotClass(textBox1, "has-error");
                AssertUI.HasNotClass(textBox2, "has-error");
                AssertUI.IsNotDisplayed(errorField);
                AssertUI.Value(textBox1, textBox2.GetValue());
            });
        }

        [Fact]
        public void Feature_Validation_DynamicValidation()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_DynamicValidation);

                // click the validate button
                browser.Last("input[type=button]").Click();

                // ensure validators are hidden
                AssertUI.InnerTextEquals(browser.Last("span"), "true");
                browser.FindElements("li").ThrowIfDifferentCountThan(0);

                // load the customer
                browser.Click("input[type=button]");
                browser.Wait();

                // try to validate
                browser.Last("input[type=button]").Click();

                browser.FindElements("li").ThrowIfDifferentCountThan(1);
                AssertUI.InnerText(browser.First("li"), s => s.Contains("Email"));

                // fix the e-mail address
                browser.Last("input[type=text]").Clear();
                browser.Last("input[type=text]").SendKeys("test@mail.com");
                browser.Last("input[type=button]").Click();

                // try to validate
                AssertUI.InnerTextEquals(browser.Last("span"), "true");
                browser.FindElements("li").ThrowIfDifferentCountThan(0);
            });
        }

        [Fact]
        public void Feature_Validation_EssentialTypeValidation()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_EssentialTypeValidation);

                var addNestedBtn = browser.ElementAt("input[type=button]", 0);
                var withBtn = browser.ElementAt("input[type=button]", 1);
                var withOutBtn = browser.ElementAt("input[type=button]", 2);

                // withnout nested test
                browser.FindElements("li").ThrowIfDifferentCountThan(0);
                withOutBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(0);
                withBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(1);
                AssertUI.InnerTextEquals(browser.First("li"), "The NullableIntegerProperty field is required.");
                withOutBtn.Click();                                         // should not remove the validation error
                browser.FindElements("li").ThrowIfDifferentCountThan(1);
                browser.First(".nullableInt input[type=text]").SendKeys("5");
                withOutBtn.Click();                                         // should not remove the validation error
                browser.FindElements("li").ThrowIfDifferentCountThan(1);
                withBtn.Click();                                            // should remove the validation error
                browser.FindElements("li").ThrowIfDifferentCountThan(0);

                // with nested test
                browser.First(".nullableInt input[type=text]").Clear();
                addNestedBtn.Click();
                withOutBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(0);
                withBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(4);
                browser.ElementAt(".nullableInt input[type=text]", 0).SendKeys("10");
                browser.ElementAt(".nullableInt input[type=text]", 2).SendKeys("10");
                withOutBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(4);
                withBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(2);

                // wrong value test
                browser.ElementAt(".nullableInt input[type=text]", 3).SendKeys("15");
                browser.First(".NaNTest input[type=text]").SendKeys("asd");
                withBtn.Click();
                browser.WaitForPostback();
                browser.FindElements("li").ThrowIfDifferentCountThan(2);
                AssertUI.InnerTextEquals(browser.First("li")
                    ,
                        "The value of property NullableFloatProperty (asd) is invalid value for type double?.");

                // correct form test
                browser.First(".NaNTest input[type=text]").Clear();
                browser.First(".NaNTest input[type=text]").SendKeys("55");
                browser.ElementAt(".nullableInt input[type=text]", 1).SendKeys("15");
                withOutBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(2);
                withBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(0);
            });
        }

        [Fact]
        public void Feature_Validation_ModelStateErrors()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_ModelStateErrors);

                //click first button - viewmodel error
                browser.ElementAt("input[type=button]", 0).Click();
                browser.FindElements(".vmErrors li").ThrowIfDifferentCountThan(1);
                AssertUI.IsNotDisplayed(browser.ElementAt(".vm1Error", 0));
                AssertUI.IsNotDisplayed(browser.ElementAt(".vm2Error", 0));
                AssertUI.IsNotDisplayed(browser.ElementAt(".vm2Error", 1));
                AssertUI.IsNotDisplayed(browser.ElementAt(".vm2Error", 2));

                //click second button - nested viewmodel1 error
                browser.ElementAt("input[type=button]", 1).Click();
                browser.FindElements(".vmErrors li").ThrowIfDifferentCountThan(1);
                AssertUI.IsDisplayed(browser.ElementAt(".vm1Error", 0));
                AssertUI.IsNotDisplayed(browser.ElementAt(".vm2Error", 0));
                AssertUI.IsNotDisplayed(browser.ElementAt(".vm2Error", 1));
                AssertUI.IsNotDisplayed(browser.ElementAt(".vm2Error", 2));

                //click third button - nested viewmodel2 two errors
                browser.ElementAt("input[type=button]", 2).Click();
                browser.FindElements(".vmErrors li").ThrowIfDifferentCountThan(2);
                AssertUI.IsNotDisplayed(browser.ElementAt(".vm1Error", 0));
                AssertUI.IsDisplayed(browser.ElementAt(".vm2Error", 0));
                AssertUI.IsNotDisplayed(browser.ElementAt(".vm2Error", 1));
                AssertUI.IsDisplayed(browser.ElementAt(".vm2Error", 2));
            });
        }

        [Fact]
        public void Feature_Validation_NestedValidation()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_NestedValidation);

                // ensure validators not visible
                AssertUI.IsNotDisplayed(browser.ElementAt("span", 0));
                AssertUI.IsDisplayed(browser.ElementAt("span", 1));
                AssertUI.ClassAttribute(browser.ElementAt("span", 1), s => !s.Contains("validator"));
                AssertUI.IsNotDisplayed(browser.ElementAt("span", 2));

                browser.FindElements(".summary1 li").ThrowIfDifferentCountThan(0);
                browser.FindElements(".summary2 li").ThrowIfDifferentCountThan(0);

                // leave textbox empty and submit the form
                browser.Click("input[type=button]");

                // ensure validators visible
                AssertUI.IsDisplayed(browser.ElementAt("span", 0));
                AssertUI.IsDisplayed(browser.ElementAt("span", 1));
                AssertUI.ClassAttribute(browser.ElementAt("span", 1), s => s.Contains("invalid"));
                AssertUI.IsDisplayed(browser.ElementAt("span", 2));

                browser.FindElements(".summary1 li").ThrowIfDifferentCountThan(0);
                browser.FindElements(".summary2 li").ThrowIfDifferentCountThan(1);

                // submit once again and test the validation summary still holds one error
                browser.Click("input[type=button]");

                // ensure validators visible
                AssertUI.IsDisplayed(browser.ElementAt("span", 0));
                AssertUI.IsDisplayed(browser.ElementAt("span", 1));
                AssertUI.ClassAttribute(browser.ElementAt("span", 1), s => s.Contains("invalid"));
                AssertUI.IsDisplayed(browser.ElementAt("span", 2));

                browser.FindElements(".summary1 li").ThrowIfDifferentCountThan(0);
                browser.FindElements(".summary2 li").ThrowIfDifferentCountThan(1);

                // fill invalid value in the task title
                browser.SendKeys("input[type=text]", "test");
                browser.Wait();
                browser.Click("input[type=button]");

                // ensure validators
                AssertUI.IsNotDisplayed(browser.ElementAt("span", 0));
                AssertUI.IsDisplayed(browser.ElementAt("span", 1));
                AssertUI.ClassAttribute(browser.ElementAt("span", 1), s => !s.Contains("validator"));
                AssertUI.IsNotDisplayed(browser.ElementAt("span", 2));

                browser.FindElements(".summary1 li").ThrowIfDifferentCountThan(0);
                browser.FindElements(".summary2 li").ThrowIfDifferentCountThan(0);

            });
        }

        [Fact]
        public void Feature_Validation_NullValidationTarget()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_NullValidationTarget);

                //get buttons
                var targetRootBtn = browser.ElementAt("input[type=button]", 0);
                var targetNullBtn = browser.ElementAt("input[type=button]", 1);
                var targetSomeBtn = browser.ElementAt("input[type=button]", 2);

                //test both fields empty
                targetRootBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(2);
                AssertUI.InnerTextEquals(browser.ElementAt("li", 0), "The NullObject field is required.");
                AssertUI.InnerTextEquals(browser.ElementAt("li", 1), "The Required field is required.");

                targetNullBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(0);

                targetSomeBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(1);
                AssertUI.InnerTextEquals(browser.First("li"), "The Required field is required.");

                //test invalid Email and empty Required
                browser.ElementAt("input[type=text]", 0).SendKeys("invalid");

                targetRootBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(2);
                AssertUI.InnerTextEquals(browser.ElementAt("li", 0), "The NullObject field is required.");
                AssertUI.InnerTextEquals(browser.ElementAt("li", 1), "The Required field is required.");

                targetNullBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(0);

                // The invalid Email won't be reported because emails are checked only on the server
                targetSomeBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(1);
                AssertUI.InnerTextEquals(browser.ElementAt("li", 0), "The Required field is required.");

                //test valid Email and empty Required
                browser.ElementAt("input[type=text]", 0).Clear();
                browser.ElementAt("input[type=text]", 0).SendKeys("valid@test.com");

                targetRootBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(2);
                AssertUI.InnerTextEquals(browser.ElementAt("li", 0), "The NullObject field is required.");
                AssertUI.InnerTextEquals(browser.ElementAt("li", 1), "The Required field is required.");

                targetNullBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(0);

                targetSomeBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(1);
                AssertUI.InnerTextEquals(browser.First("li"), "The Required field is required.");

                //test invalid Email and filled Required
                browser.ElementAt("input[type=text]", 0).Clear();
                browser.ElementAt("input[type=text]", 0).SendKeys("invalid");
                browser.ElementAt("input[type=text]", 1).SendKeys("filled");

                targetRootBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(1);
                AssertUI.InnerTextEquals(browser.ElementAt("li", 0), "The NullObject field is required.");

                targetNullBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(0);

                // The invalid email will be reported this time because now the check makes it to the server
                targetSomeBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(1);
                AssertUI.InnerTextEquals(browser.ElementAt("li", 0), "The Email field is not a valid e-mail address.");

                //test valid Email and filled Required (valid form - expect for null object)
                browser.ElementAt("input[type=text]", 0).Clear();
                browser.ElementAt("input[type=text]", 0).SendKeys("valid@test.com");

                targetRootBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(1);
                AssertUI.InnerTextEquals(browser.ElementAt("li", 0), "The NullObject field is required.");

                targetNullBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(0);

                targetSomeBtn.Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(0);
            });
        }

        [Fact]
        public void Feature_Validation_RegexValidation()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_RegexValidation);

                browser.ElementAt("input", 0).SendKeys("25");
                browser.Wait();
                browser.ElementAt("input[type=button]", 0).Click();

                AssertUI.IsNotDisplayed(browser.ElementAt("span", 0));
                AssertUI.InnerTextEquals(browser.ElementAt("span", 1), "25");

                browser.ElementAt("input", 0).SendKeys("a");
                browser.Wait();
                browser.ElementAt("input[type=button]", 0).Click();

                AssertUI.IsDisplayed(browser.ElementAt("span", 0));
                AssertUI.InnerTextEquals(browser.ElementAt("span", 1), "25");
            });
        }

        [Fact]
        public void Feature_Validation_SimpleValidation()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_SimpleValidation);

                // ensure validators not visible
                browser.WaitFor(() =>
                {
                    browser.FindElements("li").ThrowIfDifferentCountThan(0);
                }, 1000, 30);



                AssertUI.IsNotDisplayed(browser.ElementAt("span", 0));
                AssertUI.IsDisplayed(browser.ElementAt("span", 1));
                AssertUI.ClassAttribute(browser.ElementAt("span", 1), s => !s.Contains("validator"));
                AssertUI.IsNotDisplayed(browser.ElementAt("span", 2));

                // leave textbox empty and submit the form
                browser.Click("input[type=button]");

                // ensure validators visible
                browser.FindElements("li").ThrowIfDifferentCountThan(1);

                AssertUI.IsDisplayed(browser.ElementAt("span", 0));
                AssertUI.IsDisplayed(browser.ElementAt("span", 1));
                AssertUI.ClassAttribute(browser.ElementAt("span", 1), s => s.Contains("validator"));
                AssertUI.IsDisplayed(browser.ElementAt("span", 2));

                // submit once again and test the validation summary still holds one error
                browser.Click("input[type=button]");
                browser.FindElements("li").ThrowIfDifferentCountThan(1);

                // fill invalid value in the task title
                browser.SendKeys("input[type=text]", "test");
                browser.Wait();
                browser.Click("input[type=button]");

                // ensure validators visible
                browser.FindElements("li").ThrowIfDifferentCountThan(1);

                AssertUI.IsDisplayed(browser.ElementAt("span", 0));
                AssertUI.IsDisplayed(browser.ElementAt("span", 1));
                AssertUI.ClassAttribute(browser.ElementAt("span", 1), s => s.Contains("validator"));
                AssertUI.IsDisplayed(browser.ElementAt("span", 2));

                // fill valid value in the task title
                browser.ClearElementsContent("input[type=text]");
                browser.SendKeys("input[type=text]", "test@mail.com");
                browser.Wait();
                browser.Click("input[type=button]");

                // ensure validators not visible
                browser.FindElements("li").ThrowIfDifferentCountThan(0);

                AssertUI.IsNotDisplayed(browser.ElementAt("span", 0));
                AssertUI.IsDisplayed(browser.ElementAt("span", 1));
                AssertUI.ClassAttribute(browser.ElementAt("span", 1), s => !s.Contains("validator"));
                AssertUI.IsNotDisplayed(browser.ElementAt("span", 2));

                // ensure the item was added
                browser.FindElements(".table tr").ThrowIfDifferentCountThan(4);
            });
        }

        /// <summary>
        /// Feature_s the validation rules load on postback.
        /// </summary>
        [Fact]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Timeout(120000)]
        public void Feature_Validation_ValidationRulesLoadOnPostback()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_ValidationRulesLoadOnPostback);

                // click the validate button
                browser.Last("input[type=button]").Click();
                browser.Wait();

                // ensure validators are hidden
                AssertUI.InnerTextEquals(browser.Last("span"), "true");
                browser.FindElements("li").ThrowIfDifferentCountThan(0);
                // load the customer
                browser.Click("input[type=button]");

                // try to validate
                browser.Last("input[type=button]").Click();
                browser.FindElements("li").ThrowIfDifferentCountThan(1);
                AssertUI.InnerText(browser.First("li"), s => s.Contains("Email"));

                // fix the e-mail address
                browser.Last("input[type=text]").Clear();
                browser.Last("input[type=text]").SendKeys("test@mail.com");
                browser.Last("input[type=button]").Click();

                // try to validate
                AssertUI.InnerTextEquals(browser.Last("span"), "true");
                browser.FindElements("li").ThrowIfDifferentCountThan(0);
            });
        }

        [Fact]
        public void Feature_Validation_ValidationScopes()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_ValidationScopes);

                browser.First("input[type=button]").Click();

                AssertUI.InnerText(browser.First("li"), i => i.Contains("The Value field is required."));
            });
        }

        [Fact]
        public void Feature_Validation_ValidationScopes2()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_ValidationScopes2);

                // we are testing the first button

                // don't fill required field, the client validation should appear
                AssertUI.TextEquals(browser.Single(".result"), "0");
                browser.ElementAt("input[type=button]", 0).Click();
                AssertUI.TextEquals(browser.Single(".result"), "0");
                AssertUI.HasClass(browser.ElementAt("input[type=text]", 0), "has-error");
                AssertUI.HasNotClass(browser.ElementAt("input[type=text]", 1), "has-error");

                // fill first required field and second field with a short string, the server validation should appear
                browser.ElementAt("input[type=text]", 0).SendKeys("aaa");
                browser.ElementAt("input[type=text]", 1).SendKeys("aaa");
                AssertUI.TextEquals(browser.Single(".result"), "0");
                browser.ElementAt("input[type=button]", 0).Click();
                AssertUI.TextEquals(browser.Single(".result"), "0");
                AssertUI.HasNotClass(browser.ElementAt("input[type=text]", 0), "has-error");
                AssertUI.HasClass(browser.ElementAt("input[type=text]", 1), "has-error");

                // fill the second field so the validation passes
                browser.ElementAt("input[type=text]", 1).Clear().SendKeys("aaaaaa");
                AssertUI.TextEquals(browser.Single(".result"), "0");
                browser.ElementAt("input[type=button]", 0).Click();
                AssertUI.TextEquals(browser.Single(".result"), "1");
                AssertUI.HasNotClass(browser.ElementAt("input[type=text]", 0), "has-error");
                AssertUI.HasNotClass(browser.ElementAt("input[type=text]", 1), "has-error");

                // clear the fields
                browser.ElementAt("input[type=text]", 0).Clear();
                browser.ElementAt("input[type=text]", 1).Clear();

                // we are testing the second button

                // don't fill required field, the client validation should appear
                AssertUI.TextEquals(browser.Single(".result"), "1");
                browser.ElementAt("input[type=button]", 1).Click();
                AssertUI.TextEquals(browser.Single(".result"), "1");
                AssertUI.HasClass(browser.ElementAt("input[type=text]", 0), "has-error");
                AssertUI.HasNotClass(browser.ElementAt("input[type=text]", 1), "has-error");

                // fill first required field and second field with a short string, the server validation should appear
                browser.ElementAt("input[type=text]", 0).SendKeys("aaa");
                browser.ElementAt("input[type=text]", 1).SendKeys("aaa");
                AssertUI.TextEquals(browser.Single(".result"), "1");
                browser.ElementAt("input[type=button]", 1).Click();
                AssertUI.TextEquals(browser.Single(".result"), "1");
                AssertUI.HasNotClass(browser.ElementAt("input[type=text]", 0), "has-error");
                AssertUI.HasClass(browser.ElementAt("input[type=text]", 1), "has-error");

                // fill the second field so the validation passes
                browser.ElementAt("input[type=text]", 1).Clear().SendKeys("aaaaaa");
                AssertUI.TextEquals(browser.Single(".result"), "1");
                browser.ElementAt("input[type=button]", 1).Click();
                AssertUI.TextEquals(browser.Single(".result"), "2");
                AssertUI.HasNotClass(browser.ElementAt("input[type=text]", 0), "has-error");
                AssertUI.HasNotClass(browser.ElementAt("input[type=text]", 1), "has-error");

            });
        }

        [Fact]
        public void Feature_Validation_Localization()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl(SamplesRouteUrls.FeatureSamples_Validation_Localization);


                browser.ElementAt("button[type=submit]", 0).Click();

                AssertUI.TextEquals(browser.Single(".result-code"), "This comes from resource file!");
                AssertUI.TextEquals(browser.Single(".result-markup"), "This comes from resource file!");
                browser.ElementAt("a", 1).Click();
                browser.ElementAt("button[type=submit]", 0).Click();
                AssertUI.TextEquals(browser.Single(".result-code"), "Tohle pochází z resource souboru!");
                AssertUI.TextEquals(browser.Single(".result-markup"), "Tohle pochází z resource souboru!");
                browser.ElementAt("a", 0).Click();
                browser.ElementAt("button[type=submit]", 0).Click();
                AssertUI.TextEquals(browser.Single(".result-code"), "This comes from resource file!");
                AssertUI.TextEquals(browser.Single(".result-markup"), "This comes from resource file!");
            });
        }

        public ValidationTests(ITestOutputHelper output) : base(output)
        {
        }
    }
}
