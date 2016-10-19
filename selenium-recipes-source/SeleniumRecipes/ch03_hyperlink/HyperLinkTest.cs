/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
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
    public class HyperLinkTest {

        static IWebDriver driver = new FirefoxDriver();

        /// <summary>
        ///Initialize() is called once during test execution before
        ///test methods in this test class are executed.
        ///</summary>

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }
      
        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/link.html");
        }

        [TestCleanup]
        public void AfterEach()
        {
        }

        /// <summary>
        ///Cleanup() is called once during test execution after
        ///</summary>
        [ClassCleanup]
        public static void AfterAll() {
            driver.Quit();
        }


        [TestMethod]
        public void TestClickLinkByText() {
            driver.FindElement(By.LinkText("Recommend Selenium")).Click();
        }

        [TestMethod]
        public void TestClickLinkByID() {
            driver.FindElement(By.Id("recommend_selenium_link")).Click();
        }

        [TestMethod]
        public void TestClickLinkByPartialText() {
            driver.FindElement(By.PartialLinkText("Recommend Seleni")).Click();
        }

        [TestMethod]
        public void TestClickLinkByXPath() {
            driver.FindElement(By.XPath("//p/a[text()='Recommend Selenium']")).Click();
        }

        [TestMethod]
        public void TestClickLinkByXPathFunctions() {
            // Click the link (two on the same page), click the second one by narrowing down with parent div
            // using XPath contains() functions
            driver.FindElement(By.XPath("//div[contains(text(), \"Second\")]/a[text()=\"Click here\"]")).Click();
        }

        [TestMethod]
        public void TestClick3rdLinkUnderParagraphByCSS() {
            driver.FindElement(By.CssSelector("p  > a:nth-child(3)")).Click(); // the 3rd link
        }

        [TestMethod]
        public void TestClickLinkByArrayIndex() {
            Assert.IsTrue( driver.FindElements(By.LinkText("Same link")).Count == 2);
            ReadOnlyCollection<IWebElement> links = driver.FindElements(By.LinkText("Same link"));
            links[1].Click();
            Assert.IsTrue(driver.PageSource.Contains("second link page"));
        }

        [TestMethod]
        public void TestClickLinkOpenningNewWindowsOrTabRememberUrl() {
            String currentUrl = driver.Url;
            String newWindowUrl = driver.FindElement(By.LinkText("Open new window")).GetAttribute("href");
            driver.Navigate().GoToUrl(newWindowUrl);
            driver.FindElement(By.Name("name")).SendKeys("sometext");
            driver.Navigate().GoToUrl(currentUrl); // back
        }

        [TestMethod]
        public void TestClickLinkOpenningNewWindowsOrTabChangeWindow()
        {
            driver.FindElement(By.LinkText("Open new window")).Click();
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            driver.FindElement(By.Name("name")).SendKeys("on new window");
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles[0]);  // back
            driver.FindElement(By.LinkText("Recommend Selenium")).Click();
        }

        [TestMethod]
        public void TestRetrieveCommonLinkDetails() {
            Assert.AreEqual(TestHelper.SiteUrl() + "/index.html",  driver.FindElement(By.LinkText("Recommend Selenium")).GetAttribute("href") );
            Assert.AreEqual("recommend_selenium_link", driver.FindElement(By.LinkText("Recommend Selenium")).GetAttribute("id"));
            Assert.AreEqual("Recommend Selenium", driver.FindElement(By.Id("recommend_selenium_link")).Text);
            Assert.AreEqual("a", driver.FindElement(By.Id("recommend_selenium_link")).TagName);
        }

        [TestMethod]
        public void TestRetrieveAdvancedLinkDetails() {
            Assert.AreEqual("font-size: 14px;", driver.FindElement(By.Id("recommend_selenium_link")).GetAttribute("style"));
            //  Please note using attribute_value("style") won't work
            Assert.AreEqual("123", driver.FindElement(By.Id("recommend_selenium_link")).GetAttribute("data-id"));
        }

        [TestMethod]
        public void TestVerifyLinkPresent() {
            Assert.IsTrue(driver.FindElement(By.LinkText("Recommend Selenium")).Displayed);
            Assert.IsTrue(driver.FindElement(By.Id("recommend_selenium_link")).Displayed);
        }

        [TestMethod]
        public void TestVerifyLinkDsiplayedOrHidden() {
            Assert.IsTrue(driver.FindElement(By.LinkText("Recommend Selenium")).Displayed);
            Assert.IsTrue(driver.FindElement(By.Id("recommend_selenium_link")).Displayed);
            driver.FindElement(By.LinkText("Hide")).Click();

            System.Threading.Thread.Sleep(1000);
            try {
                // different from Watir, selenium returns element not found
                Assert.IsTrue(!driver.FindElement(By.LinkText("Recommend Selenium")).Displayed);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[Selenium] The hidden link cannot be found");
            }

            driver.FindElement(By.LinkText("Show")).Click();
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.LinkText("Recommend Selenium")).Displayed);
        }


    }
}