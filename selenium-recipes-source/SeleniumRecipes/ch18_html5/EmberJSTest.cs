

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
    public class EmberJSTest
    {

        static IWebDriver driver = new ChromeDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/emberjs-crud-rest/index.html");
        }

        [TestMethod]
        public void TestEmberJSDemo()
        {
            driver.FindElement(By.LinkText("Locations")).Click();
            driver.FindElement(By.LinkText("New location")).Click();
            ReadOnlyCollection<IWebElement> ember_text_fields = driver.FindElements(By.XPath("//div[@class='controls']/input[@class='ember-view ember-text-field']"));
            ember_text_fields[0].SendKeys("-24.0034583945");
            ember_text_fields[1].SendKeys("146.903459345");
            ember_text_fields[2].SendKeys("90%");

            driver.FindElement(By.XPath("//button[text() ='Update record']")).Click();
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