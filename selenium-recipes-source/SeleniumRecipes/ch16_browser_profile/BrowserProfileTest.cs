

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;
using System.IO;
using System.Collections.Generic;

namespace SeleniumRecipes
{

    [TestClass]
    public class BrowserProfileTest
    {
        static IWebDriver driver;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
        }

        [TestMethod]
        public void TestGetBrowserTypeAndVersion()
        {
            driver = new FirefoxDriver();
            ICapabilities caps = ((RemoteWebDriver)driver).Capabilities;
            String browserName = caps.BrowserName;
            String browserVersion = caps.Version;
            Console.WriteLine("browserName = " + browserName); // firefox
            Console.WriteLine("browserVersion = " + browserVersion); // 30.0
            driver.Quit();

            driver = new ChromeDriver();
            caps = ((RemoteWebDriver)driver).Capabilities;
            browserName = caps.BrowserName;
            browserVersion = caps.Version;
            Platform browserPlatform = caps.Platform;
            Console.WriteLine("browserName = " + browserName); // chrome
            Console.WriteLine("browserVersion = " + browserVersion); // 33.0.1750.152
            Console.WriteLine("browserPlatform = " + browserPlatform.ToString()); // MAC
            driver.Quit();

            // driver = new InternetExplorerDriver();
            // browserName will be "internet explorer"
        }

        // this test will fail unless have proper proxy settings
        [TestMethod]
        public void TestUseHTTPProxy()
        {
            FirefoxProfile firefoxProfile = new FirefoxProfile();
            firefoxProfile.SetPreference("network.proxy.type", 1);
            // See http://kb.mozillazine.org/Network.proxy.type

            firefoxProfile.SetPreference("network.proxy.http", "myproxy.com");
            firefoxProfile.SetPreference("network.proxy.http_port", 3128);
            driver = new FirefoxDriver(firefoxProfile);
            driver.Navigate().GoToUrl("http://itest2.com/svn-demo/");
        }

        [TestMethod]
        public void TestVerifyDownloadForChrome()
        {
            String myDownloadFolder = @"c:\temp\";
            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", myDownloadFolder);
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("http://zhimin.com/books/pwta");
            driver.FindElement(By.LinkText("Download")).Click();
            System.Threading.Thread.Sleep(10000); // wait 10 seconds for downloading to complete

            Assert.IsTrue(File.Exists(@"c:\temp\practical-web-test-automation-sample.pdf"));
        }

        [TestMethod]
        public void TestVerifyDownloadForFirefox()
        {
            String myDownloadFolder = @"c:\temp\";
            FirefoxProfile fp = new FirefoxProfile();
            fp.SetPreference("browser.download.folderList", 2);
            fp.SetPreference("browser.download.dir", myDownloadFolder);
            fp.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");            
            // disable Firefox's built-in PDF viewer
            fp.SetPreference("pdfjs.disabled", true);
            
            driver = new FirefoxDriver(fp);
            driver.Navigate().GoToUrl("http://zhimin.com/books/selenium-recipes");
            driver.FindElement(By.LinkText("Download")).Click();
            System.Threading.Thread.Sleep(10000); // wait 10 seconds for downloading to complete

            Assert.IsTrue(File.Exists(@"c:\temp\selenium-recipes-in-ruby-sample.pdf"));
        }


        [TestMethod]
        public void TestByPassBasicAuthenticationEmbeddingUserNameInURL()
        {
            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl("http://tony:password@itest2.com/svn-demo/");
            // got in, click a link
            driver.FindElement(By.LinkText("tony/")).Click();
            System.Threading.Thread.Sleep(1000);
            driver.Quit();
        }

        // Firefox Profile folder has a random string in the front of name, eg. "8yggbtss.testing"
        // This method returns the file path for a given profile name. 
        public static String GetFirefoxProfileFolderByName(String name)
        {
            string pathToCurrentUserProfiles = Environment.ExpandEnvironmentVariables("%APPDATA%") + @"\Mozilla\Firefox\Profiles"; // Path to profile
            string[] pathsToProfiles = Directory.GetDirectories(pathToCurrentUserProfiles, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var folder in pathsToProfiles)
            {
                if (folder.EndsWith(name))
                {
                    return folder;
                }
            }
            return null;
        }


        [TestMethod]
        public void TestByPassBasicAuthenticationWithAutoAuthPlugin()
        {
            /* you need have 'testing' profile set up in Firefox, see the book for instructions */
            // change to your testing foler
            FirefoxProfile firefoxProfile = new FirefoxProfile(GetFirefoxProfileFolderByName("testing"));
            firefoxProfile.AddExtension(TestHelper.ScriptDir() + @"\autoauth-2.1-fx+fn.xpi");

            driver = new FirefoxDriver(firefoxProfile);
            driver.Navigate().GoToUrl("http://itest2.com/svn-demo/");
            // got in, click a link
            driver.FindElement(By.LinkText("tony/")).Click();
            System.Threading.Thread.Sleep(1000);
            driver.Quit();
        }


        [TestMethod]
        public void TestCookies()
        {
            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl("http://travel.agileway.net");
            driver.Manage().Cookies.AddCookie(new Cookie("foo", "bar"));
            var allCookies = driver.Manage().Cookies.AllCookies;
            Cookie retrieved = driver.Manage().Cookies.GetCookieNamed("foo");
            Assert.AreEqual("bar", retrieved.Value);
        }

        [TestCleanup]
        public void After()
        {
        }

        [ClassCleanup]
        public static void AfterAll()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }


}