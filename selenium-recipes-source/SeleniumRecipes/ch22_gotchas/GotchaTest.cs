
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Interactions;

namespace SeleniumRecipes.ch22_gotchas
{
    [TestClass]
    public class GotchaTest
    {
        static IWebDriver driver = new ChromeDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/emberjs-crud-rest/index.html");
        }

        [TestMethod]
        public void TestChangeLogicBasedBrowser()
        {
            ICapabilities caps = ((RemoteWebDriver)driver).Capabilities;
            String browserName = caps.BrowserName;
            if (browserName == "chrome")
            {
                // chrome specific test statement
            }
            else if (browserName == "firefox")
            {
                // firefox specific test statement
            }
            else
            {
                throw new Exception("Unsupported browser: " + browserName);
            }
        }
    }
}
