

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
    public class FrameTest
    {

        static IWebDriver driver = new FirefoxDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/frames.html");
        }

        [TestMethod]
        public void TestFrames()
        {
            driver.SwitchTo().Frame("topNav"); // name
            driver.FindElement(By.LinkText("Menu 2 in top frame")).Click();

            // need to switch to default before another switch
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame("menu_frame"); // not working for Chrome, fine for Firefox
            driver.FindElement(By.LinkText("Green Page")).Click();

            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame("content");
            driver.FindElement(By.LinkText("Back to original page")).Click();
        }

        [TestMethod]
        public void TestIFrame()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/iframe.html");
            driver.FindElement(By.Name("user")).SendKeys("agileway");

            driver.SwitchTo().Frame("Frame1"); // name
            driver.FindElement(By.Name("username")).SendKeys("tester");
            driver.FindElement(By.Name("password")).SendKeys("TestWise");
            driver.FindElement(By.Id("loginBtn")).Click();
            Assert.IsTrue(driver.PageSource.Contains("Signed in"));
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.Id("accept_terms")).Click();
        }

        [TestMethod]
        public void TestIFrameByIndex()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/iframes.html");
            driver.SwitchTo().Frame(0);
            driver.FindElement(By.Name("username")).SendKeys("agileway");
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(1);
            driver.FindElement(By.Id("radio_male")).Click();
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
