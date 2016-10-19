

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;

namespace SeleniumRecipes
{

    [TestClass]
    public class AngularJSTest
    {

        static IWebDriver driver = new ChromeDriver();

        String site_root_url;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/angular_todo.html");
        }

        [TestMethod]
        public void TestAddTodoItemAndMarkItDone()
        {
            Assert.IsTrue(driver.PageSource.Contains("1 of 2 remaining"));
            driver.FindElement(By.XPath("//input[@ng-model='todoText']")).SendKeys("Learn test automation");
            driver.FindElement(By.XPath("//input[@type = 'submit' and @value='add']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElements(By.XPath("//input[@type = 'checkbox' and @ng-model='todo.done']"))[2].Click();
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(driver.PageSource.Contains("1 of 3 remaining"));
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