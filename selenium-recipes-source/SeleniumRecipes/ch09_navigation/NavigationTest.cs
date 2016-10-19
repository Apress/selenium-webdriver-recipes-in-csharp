

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;

using System.Collections.ObjectModel;

namespace SeleniumRecipes
{
    [TestClass]
    public class NavigationTest
    {

        static IWebDriver driver = new FirefoxDriver();
       
        String siteRootUrl;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/index.html");
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
        public void TestGotoURL()
        {
            driver.Navigate().GoToUrl("https://google.com");
        }

        [TestMethod]
        public void TestSetURL()
        {
            driver.Url = "http://testwisely.com";
        }

        [TestMethod]
        public void TestBackRefreshForward()
        {
            driver.Navigate().Back();
            driver.Navigate().Refresh();
            driver.Navigate().Forward();
        }

        [TestMethod]
        public void TestResizeWindow()
        {
            driver.Manage().Window.Size = new System.Drawing.Size(1024, 768);
        }

        [TestMethod]
        public void TestMaximizeWindow()
        {
            driver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(1000);
            driver.Manage().Window.Size = new System.Drawing.Size(1024, 768);
        }

        [TestMethod]
        public void TestMinimizeBrowser()
        {
            driver.Manage().Window.Position = new System.Drawing.Point(-2000, 0);
            driver.FindElement(By.LinkText("Hyperlink")).Click();
            System.Threading.Thread.Sleep(2000);
            driver.Manage().Window.Position = new System.Drawing.Point(0, 0);
        }

        [TestMethod]
        public void TestMoveBrowser()
        {
            driver.Manage().Window.Position = new System.Drawing.Point(100, 100); 
            System.Threading.Thread.Sleep(1000);
            driver.Manage().Window.Position = new System.Drawing.Point(0, 0);
        }

        public void Visit(String path)
        {
            driver.Navigate().GoToUrl(siteRootUrl + path);
        }

        [TestMethod]
        public void TestGoToPageWithinSiteUsingFunction()
        {
            Visit("/demo");
            Visit("/demo/survey");
            Visit("/"); // home page   
        }

        [TestMethod]
        public void TestSwitchWindowOrTab()
        {
            driver.FindElement(By.LinkText("Hyperlink")).Click();
            driver.FindElement(By.LinkText("Open new window")).Click();
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            String firstTab = (String)windowHandles[0];
            String lastTab = windowHandles[windowHandles.Count - 1];
            driver.SwitchTo().Window(lastTab);
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("This is url link page"));
            driver.SwitchTo().Window(firstTab); // back to first tab/window
            Assert.IsTrue(driver.FindElement(By.LinkText("Open new window")).Displayed);
        }

        [TestMethod]
        public void TestScrollToElement()
        {
            driver.FindElement(By.LinkText("Button")).Click();
            driver.Manage().Window.Size = new System.Drawing.Size(1024, 768);            
            IWebElement elem = driver.FindElement(By.Name("submit_action_2"));
            int elemPos = elem.Location.Y;
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scroll(0, " + elemPos + ");");
            System.Threading.Thread.Sleep(2000);
            elem.Click();            
        }


    }

}