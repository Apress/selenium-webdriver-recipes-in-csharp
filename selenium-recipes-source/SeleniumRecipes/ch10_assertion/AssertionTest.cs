

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
    public class AssertionTest
    {

        static IWebDriver driver = new FirefoxDriver();
        String url;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/assert.html");
        }

        [TestMethod]
        public void TestAssertTitle()
        {
            Assert.AreEqual("Assertion Test Page", driver.Title);
        }

        [TestMethod]
        public void TestAssertPageText()
        {
            String matching_str = "Text assertion with a  (tab before), and \r\n(new line before)!";
            // watir IE repalce \n with \r\n
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains(matching_str));
        }

        [TestMethod]
        public void TestAssertPageSource()
        {
            String html_fragment = "Text assertion with a  (<b>tab</b> before), and \r\n(new line before)!";
            Assert.IsTrue(driver.PageSource.Contains(html_fragment));
        }

        [TestMethod]
        public void TestAssertLabelText()
        {
            Assert.AreEqual("First Label", driver.FindElement(By.Id("label_1")).Text);
            Assert.AreEqual("Second Span", driver.FindElement(By.Id("span_2")).Text);
        }

        [TestMethod]
        public void TestAssertDivText()
        {
            Assert.AreEqual("TestWise", driver.FindElement(By.Id("div_child_1")).Text);
            Assert.AreEqual("Wise Products\r\nTestWise\r\nBuildWise", driver.FindElement(By.Id("div_parent")).Text);
        }

        [TestMethod]
        public void TestAssertDivHtml()
        {
            IWebElement the_element = driver.FindElement(By.Id("div_parent"));
            Object html = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].outerHTML;", the_element);
            Console.WriteLine("html = " + html);
            String expected = "<div id=\"div_parent\">\r\n"
                    + "	   Wise Products\r\n"
                    + "	   <div id=\"div_child_1\">\r\n"
                    + "	   	 TestWise\r\n"
                    + "	   </div>\r\n"
                    + "	   <div id=\"div_child_2\">\r\n"
                    + "	   	 BuildWise\r\n"
                    + "	   </div>\r\n"
                    + "	 </div>";
            Assert.AreEqual(expected, html);
        }

        [TestMethod]
        public void TestAssertTextInTable()
        {
            IWebElement the_element = driver.FindElement(By.Id("alpha_table"));
            Assert.AreEqual("A B\r\na b", the_element.Text);
            Object html = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].outerHTML;", the_element);
            String htmlStr = (String)html;
            Assert.IsTrue(htmlStr.Contains("<td id=\"cell_1_1\">A</td>"));
        }

        [TestMethod]
        public void TestAssertTextInTableCell()
        {
            Assert.AreEqual("A", driver.FindElement(By.Id("cell_1_1")).Text);
        }

        [TestMethod]
        public void TestAssertTextInTableCellWithIndex()
        {
            Assert.AreEqual("b", driver.FindElement(By.XPath("//table/tbody/tr[2]/td[2]")).Text);
        }

        [TestMethod]
        public void TestAssertTextInTableRow()
        {
            Assert.AreEqual("A B", driver.FindElement(By.Id("row_1")).Text);
        }


        [TestMethod]
        public void TestAssertImagePresents()
        {
            Assert.IsTrue(driver.FindElement(By.Id("next_go")).Displayed);
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

