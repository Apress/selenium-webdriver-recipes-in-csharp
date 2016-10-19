
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;


namespace SeleniumRecipes.ch16_browser
{
    [TestClass]
    public class PhantomJSDriverTest
    {
        static IWebDriver driver = new PhantomJSDriver();

        [ClassCleanup]
        public static void AfterAll()
        {            
           driver.Quit();            
        }

        [TestMethod]
        public void TestHeadlessMode()
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
        }
    }
}
