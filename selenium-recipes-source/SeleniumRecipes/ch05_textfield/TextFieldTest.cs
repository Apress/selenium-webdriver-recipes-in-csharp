

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace SeleniumRecipes
{

    [TestClass]
    public class TextFieldTest
    {

        static IWebDriver driver = new FirefoxDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/text_field.html");
        }

        [TestMethod]
        public void TestEnterTextByName()
        {
            driver.FindElement(By.Name("username")).SendKeys("agileway");
        }

        [TestMethod]
        public void TestEnterTextByID()
        {
            driver.FindElement(By.Id("user")).SendKeys("agileway");
        }

        [TestMethod]
        public void TestEnterPasswordByID()
        {
            driver.FindElement(By.Id("pass")).SendKeys("testisfun");
        }

        [TestMethod]
        public void TestEnterTextAreaMultiLine()
        {
            // \n is a new line character
            driver.FindElement(By.Id("comments")).SendKeys("Automaated testing is\r\nFun!");
        }

        [TestMethod]
        public void TestClearTextField()
        {
            driver.FindElement(By.Id("user")).SendKeys("agileway");
            driver.FindElement(By.Name("username")).Clear();
        }

        // Selenium, different from Watir, does not have focus
        // Instead, calling SendKeys will focus on th element
        public void TestFocusTextField()
        {
            driver.FindElement(By.Id("pass")).SendKeys("");
        }

        public void TestFocusTextFieldUsingJavaScript()
        {
            IWebElement elem = driver.FindElement(By.Id("pass"));
		        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", elem);
        }
				
        [TestMethod]
        public void TestVerifyValueInTextField()
        {
            driver.FindElement(By.Id("user")).SendKeys("testwisely");
            Assert.AreEqual("testwisely", driver.FindElement(By.Id("user")).GetAttribute("value"));
        }

        [TestMethod]
        public void TestReadOnlyTextFields() {
            // the below won't work for read_only text fields
            // driver.FindElement(:id, "writable").send_keys("new value")
            ((IJavaScriptExecutor)driver).ExecuteScript("$('#readonly_text').val('bypass');");
            Assert.AreEqual("bypass", driver.FindElement(By.Id("readonly_text")).GetAttribute("value"));

            ((IJavaScriptExecutor)driver).ExecuteScript("$('#disabled_text').val('anyuse');");
            Assert.AreEqual("anyuse", driver.FindElement(By.Id("disabled_text")).GetAttribute("value"));
        }

        [TestCleanup]
        public void After()
        {
        }

        [ClassCleanup]
        public static void AfterAll()
        {
            driver.Quit();
        }
    }

}