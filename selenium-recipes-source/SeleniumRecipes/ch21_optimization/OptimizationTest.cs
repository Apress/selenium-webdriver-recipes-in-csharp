
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
using System.Diagnostics;

namespace SeleniumRecipes.ch21_optimization
{
    [TestClass]
    public class OptimizationTest
    {

        static IWebDriver driver = new ChromeDriver();

        
        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/text_field.html");
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
        public void TestUsingTernaryOperator()
        {
            string refNo = driver.FindElement(By.Id("ref_no")).Text;
            if (refNo.Contains("VIP")) { // Special 
                Assert.AreEqual("Please go upstair", driver.FindElement(By.Id("notes")).Text);
            }
            else
            {
                Assert.AreEqual("", driver.FindElement(By.Id("notes")).Text);
            }

            // using tenary operation
            Assert.AreEqual(refNo.Contains("VIP") ? "Please go upstair" : "", driver.FindElement(By.Id("notes")).Text);
        }

        [TestMethod]
        public void TestPasteLargeTextInTextArea()
        {
            var watch = Stopwatch.StartNew();
            string longText= new string('*', 5000);
            IWebElement textArea = driver.FindElement(By.Id("comments"));
            textArea.SendKeys(longText);
            watch.Stop();
            var elapsedSec = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("time cost by SendKeys: " + elapsedSec + " seconds");

            textArea.Clear();
            watch = Stopwatch.StartNew();
            ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementById('comments').value = arguments[0];", longText);
            watch.Stop();
            elapsedSec = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("time cost by JS set: " + elapsedSec + " seconds");
        }
    }
}
