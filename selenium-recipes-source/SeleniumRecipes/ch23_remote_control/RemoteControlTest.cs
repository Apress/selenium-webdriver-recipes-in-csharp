
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace SeleniumRecipes
{

    public class RemoteControlTest
    {

        static IWebDriver driver;

        [TestInitialize]
        public static void BeforeAll()
        {
            try
            {
                DesiredCapabilities capabilities = DesiredCapabilities.Firefox();
                driver = new RemoteWebDriver(capabilities);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex = " + ex);
            }
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl("http://testwisely.com/demo/netbank");
        }

        [TestMethod]
        public void TestExplicitWaitsInRemoteBrowser()
        {
            SelectElement select = new SelectElement(driver.FindElement(By.Name("account")));
            select.SelectByText("Cheque");
            driver.FindElement(By.Id("rcptAmount")).SendKeys("250");
            driver.FindElement(By.XPath("//input[@value='Transfer']")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));  // seconds
            wait.Until(d => d.FindElement(By.Id("receiptNo")));
        }

        [TestMethod]
        public void TestImplicitWaitsInRemoteBrowser()
        {
            SelectElement select = new SelectElement(driver.FindElement(By.Name("account")));
            select.SelectByText("Cheque");
            driver.FindElement(By.Id("rcptAmount")).SendKeys("250");
            driver.FindElement(By.XPath("//input[@value='Transfer']")).Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            Assert.IsTrue(driver.FindElement(By.Id("receiptNo")).Text.Length > 0);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1)); //reset for later steps
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