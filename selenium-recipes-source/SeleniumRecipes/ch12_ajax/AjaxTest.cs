

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace SeleniumRecipes
{
    [TestClass]
    public class AjaxTest
    {

        static IWebDriver driver = new FirefoxDriver();

        String site_root_url;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl("http://testwisely.com/demo/netbank");
        }

        [TestMethod]
        public void TestWaitUsingSleep()
        {
            SelectElement select = new SelectElement(driver.FindElement(By.Name("account")));
            select.SelectByText("Cheque");
            driver.FindElement(By.Id("rcptAmount")).SendKeys("250");
            driver.FindElement(By.XPath("//input[@value='Transfer']")).Click();
            System.Threading.Thread.Sleep(10000);
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Receipt No:"));
        }

        [TestMethod]
        public void TestExplicitWaits()
        {
            SelectElement select = new SelectElement(driver.FindElement(By.Name("account")));
            select.SelectByText("Cheque");
            driver.FindElement(By.Id("rcptAmount")).SendKeys("250");
            driver.FindElement(By.XPath("//input[@value='Transfer']")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));  // seconds
            wait.Until(d => d.FindElement(By.Id("receiptNo")) );
        }

        [TestMethod]
        public void TestImplicitWaits()
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

        [TestMethod]
        public void TestWaitUsingJQueryActiveFlag()
        {
            driver.Navigate().GoToUrl("http://travel.agileway.net");
            driver.FindElement(By.Id("username")).SendKeys("agileway");
            driver.FindElement(By.Id("password")).SendKeys("testwise");
            driver.FindElement(By.XPath("//input[@value='Sign in']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//input[@name='tripType' and @value='oneway']")).Click();
            new SelectElement(driver.FindElement(By.Name("fromPort"))).SelectByText("New York");
            new SelectElement(driver.FindElement(By.Name("toPort"))).SelectByText("Sydney");
            new SelectElement(driver.FindElement(By.Id("departDay"))).SelectByText("04");
            new SelectElement(driver.FindElement(By.Id("departMonth"))).SelectByText("March 2012");            
            driver.FindElement(By.XPath("//input[@value='Continue']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.Name("passengerFirstName")).SendKeys("Wise");
            driver.FindElement(By.Name("passengerLastName")).SendKeys("Tester");
            driver.FindElement(By.XPath("//input[@value='Next']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//input[@name='card_type' and @value='visa']")).Click();
            driver.FindElement(By.Name("card_number")).SendKeys("4000000000000000");
            new SelectElement(driver.FindElement(By.Name("expiry_year"))).SelectByText("2016");
            driver.FindElement(By.XPath("//input[@value='Pay now']")).Click();
            WaitForAjaxComplete(11);
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Booking number"));
        }

        public void WaitForAjaxComplete(int maxSeconds)
        {
            bool is_ajax_compete = false;
            for (int i = 1; i <= maxSeconds; i++)
            {
                is_ajax_compete = (bool) ((IJavaScriptExecutor)driver).ExecuteScript("return jQuery.active == 0");
                if (is_ajax_compete)
                {
                    return;
                }
                System.Threading.Thread.Sleep(1000);
            }
           throw new Exception("Timed out waiting for AJAX call after " + maxSeconds + " seconds");
        }
    }

}