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
    public class OptimizationAssertionTest
    {
        static IWebDriver driver = new FirefoxDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/WebDriverStandard.html");
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
        public void TestSearchTextInPageSourceFaster()
        {
            var watch = Stopwatch.StartNew();
            String checkText = "platform- and language-neutral wire protocol";
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains(checkText));
            watch.Stop();
            var elapsedSec = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Method 1: Search page text took " + elapsedSec + " seconds");

            watch = Stopwatch.StartNew();
            Assert.IsTrue(driver.PageSource.Contains(checkText));
            elapsedSec = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Method 2: Search page HTML took " + elapsedSec + " seconds");

            // Compare the difference 
            // Method 1: Search page text took 17.192 seconds
            // Method 2: Search page HTML took 0.582 seconds
        }

        [TestMethod]
        public void TestUseSpecificElementForAssertion()
        {
            var watch = Stopwatch.StartNew();
            String checkText = "platform- and language-neutral wire protocol";
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains(checkText));
            watch.Stop();
            var elapsedSec = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Method 1: Search page text took " + elapsedSec + " seconds");

            watch = Stopwatch.StartNew();
            Assert.IsTrue(driver.FindElement(By.Id("abstract")).Text.Contains(checkText));
            watch.Stop();
            elapsedSec = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Method 2: Search specific element took " + elapsedSec + " seconds");
        }


        [TestMethod]
        public void TestUseLocalVariableToSpeedUp()
        {
            var watch = Stopwatch.StartNew();
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Firefox"));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("chrome"));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("W3C"));
            watch.Stop();
            var elapsedSec = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Method 1: 3 assertions took " + elapsedSec + " seconds");

            // a much more efficient way
            watch = Stopwatch.StartNew();
            var thePageText = driver.FindElement(By.TagName("body")).Text;
            Assert.IsTrue(thePageText.Contains("Firefox"));
            Assert.IsTrue(thePageText.Contains("chrome"));
            Assert.IsTrue(thePageText.Contains("W3C"));
            watch.Stop();
            elapsedSec = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Method 2: 3 assertions took " + elapsedSec + " seconds");
        }



    }
}
