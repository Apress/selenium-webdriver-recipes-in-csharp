

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
    public class CheckBoxTest
    {

        static IWebDriver driver = new ChromeDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/checkbox.html");
        }

        [TestMethod]
        public void TestCheckByName()
        {
            driver.FindElement(By.Name("vehicle_bike")).Click();
            driver.FindElement(By.Name("vehicle_car")).Click();
        }

        [TestMethod]
        public void TestCheckByID()
        {
            IWebElement the_checkbox = driver.FindElement(By.Id("checkbox_car"));
            if (!the_checkbox.Selected)
            {
                the_checkbox.Click();
            }
        }

        [TestMethod]
        public void TestUncheck()
        {
            IWebElement the_checkbox = driver.FindElement(By.Name("vehicle_bike"));
            the_checkbox.Click();
            // can't use clear, that for text field
            // browser.FindElement(By.Id("checkbox_bike")).clear
            if (the_checkbox.Selected)
            {
                the_checkbox.Click();
            }
        }

        [TestMethod]
        public void TestVerifyCheckBoxSelected()
        {
            IWebElement the_checkbox = driver.FindElement(By.Name("vehicle_bike"));
            Assert.IsFalse(the_checkbox.Selected);
            the_checkbox.Click();
            Assert.IsTrue(the_checkbox.Selected);
        }


        [TestMethod]
        public void testCustomizeiCheckCheckboxes()
        {
            // Error: Element is not clickable
            // driver.FindElement(By.Id("q2_1")).click();
            driver.FindElements(By.ClassName("icheckbox_square-red"))[0].Click();
            driver.FindElements(By.ClassName("icheckbox_square-red"))[1].Click();

            // More precise with XPath
            driver.FindElement(By.XPath("//div[contains(@class, 'icheckbox_square-red')]/input[@type='checkbox' and @value='Soccer']/..")).Click();
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