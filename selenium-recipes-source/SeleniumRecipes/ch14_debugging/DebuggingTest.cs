

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.IO;

namespace SeleniumRecipes
{
    [TestClass]
    public class DebuggingTest
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
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/assert.html");
        }

        [TestMethod]
        public void TestPrintOutText()
        {
            Console.WriteLine("Now on page: " + driver.Title);
            String app_no = driver.FindElement(By.Id("app_id")).Text;
            Console.WriteLine("Application number is " + app_no);
        }

        [TestMethod]
        public void TestWritePageOrElementHtmlToFile()
        {
            using (StreamWriter outfile = new StreamWriter(TestHelper.TempDir() + @"\login_page.html"))
            {
                outfile.Write(driver.PageSource);
            }


            IWebElement the_element = driver.FindElement(By.Id("div_parent"));
            String the_element_html = (String)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].outerHTML;", the_element);

            using (StreamWriter outfile = new StreamWriter(TestHelper.TempDir() + @"\login_parent.xhtml"))
            {
                outfile.Write(the_element_html);
            }

        }

        [TestMethod]
        public void TestTakeScreenshot()
        {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(TestHelper.TempDir() + @"\screenshot.png", System.Drawing.Imaging.ImageFormat.Png);        
        }

        [TestMethod]
        public void TestTakeScreenshotWithTimestamp()
        {
            ITakesScreenshot ssdriver = driver as ITakesScreenshot;
            Screenshot screenshot = ssdriver.GetScreenshot();
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd-hhmm-ss");
            screenshot.SaveAsFile(TestHelper.TempDir() + @"\Exception-" + timestamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
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