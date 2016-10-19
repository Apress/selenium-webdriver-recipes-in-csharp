

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace SeleniumRecipes
{
    [TestClass]
    public class SelectTest
    {

        static IWebDriver driver = new FirefoxDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/select_list.html");
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
        public void TestSelectOptionByLabel()
        {
            // SelectElement defined in OpenQA.Selenium.Support.UI namespace the word Select is already used in C# that is why its implementation is changed and class is named differently.
            IWebElement elem = driver.FindElement(By.Name("car_make"));
            SelectElement select = new SelectElement(elem);
            select.SelectByText("Volvo (Sweden)");

            // one line
           new SelectElement(driver.FindElement(By.Name("car_make"))).SelectByText("Honda (Japan)");
        }

        [TestMethod]
        public void TestSelectOptionByValue()
        {
            SelectElement select = new SelectElement(driver.FindElement(By.Id("car_make_select")));
            select.SelectByValue("audi");
        }


        [TestMethod]
        public void TestSelectOptionByIndex()
        {
            SelectElement select = new SelectElement(driver.FindElement(By.Id("car_make_select")));
            select.SelectByIndex(1); // 0 based index
        }

        [TestMethod]
        public void TestSelectByIteratingOption()
        {
            IWebElement selectElem = driver.FindElement(By.Id("car_make_select"));
            foreach (IWebElement option in selectElem.FindElements(By.TagName("option")))
            {
                if (option.Text.Equals("Volvo (Sweden)"))
                {
                    option.Click();
                }
            }

        }

        [TestMethod]
        public void TestVerifyOptionInSelectList()
        {
            IWebElement selectElem = driver.FindElement(By.Id("car_make_select"));
            IList<String> selectLabels = new List<String>();
            IList<String> selectValues = new List<String>();

            foreach (IWebElement option in selectElem.FindElements(By.TagName("option")))
            {
                selectLabels.Add(option.Text);
                selectValues.Add(option.GetAttribute("value"));
            }

            Assert.IsTrue(selectLabels.Contains("Audi (Germany)"));
            Assert.IsTrue(selectValues.Contains("audi"));
        }

        [TestMethod]
        public void TestVerifySelectedValue() {
        SelectElement select = new SelectElement(driver.FindElement(By.Id("car_make_select")));
        select.SelectByText("Volvo (Sweden)");
        Assert.AreEqual("volvo", select.SelectedOption.GetAttribute("value"));
    }

        [TestMethod]
        public void TestVerifySelectedText() {
            SelectElement select = new SelectElement(driver.FindElement(By.Id("car_make_select")));
        select.SelectByValue("audi");
        Assert.AreEqual("Audi (Germany)", select.SelectedOption.Text);
    }

        [TestMethod]
        public void TestSelectMultiple() {
            IWebElement elem = driver.FindElement(By.Name("test_framework"));
            SelectElement select = new SelectElement(elem);
            select.SelectByText("Selenium");
            select.SelectByValue("rwebspec");
            select.SelectByIndex(2);
            Assert.AreEqual(3, select.AllSelectedOptions.Count);
        }

        [TestMethod]
        public void TestDeselectOption() {
            IWebElement elem = driver.FindElement(By.Name("test_framework"));
            SelectElement select = new SelectElement(elem);
            select.SelectByText("RWebSpec");
            select.SelectByText("Selenium");
            select.DeselectByText("RWebSpec");
            select.DeselectByValue("selenium");
            // one more 
            // select.deselectByIndex(0);
            Assert.AreEqual(0, select.AllSelectedOptions.Count);
        }

        [TestMethod]
        public void TestDeselectMultipleAllOptions() {
            IWebElement elem = driver.FindElement(By.Name("test_framework"));
            SelectElement select = new SelectElement(elem);
            select.SelectByText("Selenium");
            select.SelectByText("RWebSpec");
            select.DeselectAll();
            Assert.IsTrue(select.AllSelectedOptions.Count == 0 );
        }

        [TestMethod]
        public void TestAssertMultipleSelectedOptions()
        {
            SelectElement select = new SelectElement(driver.FindElement(By.Name("test_framework")));
            select.SelectByText("Selenium");
            select.SelectByText("RWebSpec");

            IList<IWebElement> selected = select.AllSelectedOptions;
            Assert.AreEqual(2, selected.Count);
            Assert.AreEqual("RWebSpec", selected[0].Text); // The order is based on the option list
            Assert.AreEqual("Selenium", selected[1].Text);
        }


    }

}