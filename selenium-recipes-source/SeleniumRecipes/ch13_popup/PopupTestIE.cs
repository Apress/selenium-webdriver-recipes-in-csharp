
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Net;
using System.Web;

/* To successfully run some test cases in this file, requires BuildWise Agent free Popup Hanlder running */
namespace SeleniumRecipes
{
    [TestClass]
    public class PopupTestIE
    {

        static IWebDriver driver;

        String site_root_url;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver = new InternetExplorerDriver();
            driver.Navigate().GoToUrl(TestHelper.siteUrl() +  "/popup.html");
        }

        [TestMethod]
        public void TestFileUpload()
        {
            String filePath = TestHelper.scriptDir() + @"\testdata\users.csv";
            driver.FindElement(By.Name("document[file]")).SendKeys(filePath);
        }

        [TestMethod]
        public void TestIEModalDialog()
        {
            driver.FindElement(By.LinkText("Show Modal Dialog")).Click();

            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            String mainWin = windowHandles[0]; // first one is the main window
            String modalWin = windowHandles[windowHandles.Count - 1];

            driver.SwitchTo().Window(modalWin);
            driver.FindElement(By.Name("user")).SendKeys("in-modal");
            driver.SwitchTo().Window(mainWin);
            driver.FindElement(By.Name("status")).SendKeys("Done");
        }

        [TestMethod]
        public void TestJavaScriptPopup()
        {
            driver.Navigate().GoToUrl("http://testwisely.com/demo/popups");
            notifyPopupHandlerJavaScript("Message from webpage");
            driver.FindElement(By.Id("buy_now_btn")).Click();
            System.Threading.Thread.Sleep(15000);
            driver.FindElement(By.LinkText("NetBank")).Click();

            // don't use Waits, it somehow causes Selenium to click the cancel butotn
            // WebDriverWait wait = new WebDriverWait(driver, 20);  // seconds
            //wait.until(ExpectedConditions.presenceOfElementLocated(By.LinkText("NetBank"))); // on next page
        }

        /* The test should fail */
        [TestMethod]
        [Timeout(5000)]
        public void TestTimeout()
        {
            driver.Navigate().GoToUrl("http://testwisely.com/demo/popups");
            notifyPopupHandlerJavaScript("Message from webpage");
            driver.FindElement(By.Id("buy_now_btn")).Click();
            System.Threading.Thread.Sleep(15000); // to fail the test
            driver.FindElement(By.LinkText("NetBank")).Click();
        }


        [TestMethod]
        public void TestBasicAuthenticationDialog()
        {
            String winTitle = "Windows Security";
            String username = "tony";
            String password = "password";
            notifyPopupHandlerBasicAuth(winTitle, username, password);
            driver.Navigate().GoToUrl("http://itest2.com/svn-demo/");
            System.Threading.Thread.Sleep(20000);
            driver.FindElement(By.LinkText("tony/")).Click();
        }

        public static String getUrlText(String path)
        {
            String handlerURL = "http://localhost:4208";
            try
            {
                Uri  website = new Uri(handlerURL + path);
                String urlContent = null;
                using (WebClient client = new WebClient())
                {
                    urlContent = client.DownloadString(website);
                }
                return urlContent;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                return "Error";

            }
        }

        public static void notifyPopupHandlerBasicAuth(String winTitle, String username, String password)
        {
            
            String handlerPath = "/basic_authentication/win_title=" + HttpUtility.UrlEncode(winTitle) + "&user=" + username + "&password=" + password;
            getUrlText(handlerPath);
        }

        public static void notifyPopupHandlerJavaScript(String winTitle)
        {
            String handlerPath = "/popup/win_title=" + HttpUtility.UrlEncode(winTitle);
            getUrlText(handlerPath);
        }

        [TestCleanup]
        public void After()
        {
            driver.Quit();
        }

        [ClassCleanup]
        public static void AfterAll()
        {
        }
    }

}