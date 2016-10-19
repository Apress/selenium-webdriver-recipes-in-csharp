
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
    public class GridTest
    {

        static IWebDriver driver;

        // Make sure the hub and the nodes are up running 
        [TestMethod]
        public void TestRunningInGrid()
        {
            DesiredCapabilities capabilities = DesiredCapabilities.Chrome();
            driver = new RemoteWebDriver(new Uri("http://127.0.0.1:4444/wd/hub"), capabilities);
            driver.Navigate().GoToUrl("http://testwisely.com/demo/netbank");
            SelectElement select = new SelectElement(driver.FindElement(By.Name("account")));
            select.SelectByText("Cheque");
            driver.FindElement(By.Id("rcptAmount")).SendKeys("250");
            driver.FindElement(By.XPath("//input[@value='Transfer']")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));  // seconds
            wait.Until(d => d.FindElement(By.Id("receiptNo")));
        }

    }

}