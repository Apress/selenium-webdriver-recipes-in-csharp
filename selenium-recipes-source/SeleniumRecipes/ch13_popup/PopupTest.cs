

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
    public class PopupTest
    {

        static IWebDriver driver = new FirefoxDriver();

        String site_root_url;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/popup.html");
        }


        [TestMethod]
        [Timeout(30 * 1000)] // 30 seconds
        public void TestTimeOut()
        {
            System.Threading.Thread.Sleep(2000);
            System.Console.WriteLine("Completed");
        }

        
        [TestMethod]
        public void TestFileUpload()
        {
            String filePath = TestHelper.ScriptDir() + @"\testdata\users.csv";
            driver.FindElement(By.Name("document[file]")).SendKeys(filePath);
        }

        [TestMethod]
        public void TestJavaScriptAlert()
        {
            driver.Navigate().GoToUrl("http://testwisely.com/demo/popups");
            driver.FindElement(By.XPath("//input[contains(@value, 'Buy Now')]")).Click();
            IAlert a = driver.SwitchTo().Alert();
            if (a.Text.Equals("Are you sure"))
            {
                a.Accept();
            }
            else
            {
                a.Dismiss();
            }
        }

        [TestMethod]
        public void TestJavaScriptAlertWithJavaScript()
        {
            driver.Navigate().GoToUrl("http://testwisely.com/demo/popups");
            ((IJavaScriptExecutor)driver).ExecuteScript("window.confirm = function() { return true; }");
            ((IJavaScriptExecutor)driver).ExecuteScript("window.alert = function() { return true; }");
            ((IJavaScriptExecutor)driver).ExecuteScript("window.prompt = function() { return true; }");
            driver.FindElement(By.XPath("//input[contains(@value, 'Buy Now')]")).Click();

        }

        [TestMethod]
        public void TestModalDialog()
        {
            driver.FindElement(By.Id("bootbox_popup")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//div[@class='modal-footer']/button[text() = 'OK']")).Click();
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