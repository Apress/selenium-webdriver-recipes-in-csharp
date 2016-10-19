
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
    public class FirefoxProfileTest
    {

        // More preferences: https://code.google.com/p/selenium/wiki/FirefoxDriver
        [TestMethod]
        public void TestCustomizeProfile()
        {
            FirefoxProfile fp = new FirefoxProfile();
            fp.SetPreference("webdriver.load.strategy", "unstable"); // As of 2.19. from 2.9 - 2.18 use 'fast'
            IWebDriver driver = new FirefoxDriver(fp);
            driver.Navigate().GoToUrl("http://testwisely.com/demo");
            driver.Quit();
        }

        // More preferences: https://code.google.com/p/selenium/wiki/FirefoxDriver
        [TestMethod]
        public void TestUsingPresetProfile()
        {
            // the getting profile is different from Java, which use ProfilesIni
            //ProfilesIni profile = new ProfilesIni();
            //FirefoxProfile ffprofile = profile.getProfile("testing");
            IWebDriver driver = null;

            string pathToCurrentUserProfiles = Environment.ExpandEnvironmentVariables("%APPDATA%") + @"\Mozilla\Firefox\Profiles"; // Path to profile
            string[] pathsToProfiles = Directory.GetDirectories(pathToCurrentUserProfiles, "*.testing", SearchOption.TopDirectoryOnly);
            if (pathsToProfiles.Length != 0)
            {
                FirefoxProfile profile = new FirefoxProfile(pathsToProfiles[0]);
                profile.SetPreference("browser.tabs.loadInBackground", false); // set preferences you need
                driver = new FirefoxDriver(new FirefoxBinary(), profile);
            }

            driver.Navigate().GoToUrl("http://testwisely.com/demo");
            driver.Quit();
        }

        [TestMethod]
        public void TestLoadExtension()
        {
            String extensionFilePath = TestHelper.scriptDir() + @"\test\ch08_navigation\firebug-1.12.5-fx.xpi";
            File extensionFile = new File(extensionFilePath);
            FirefoxProfile fp = new FirefoxProfile();
            fp.EnableNativeEvents = true;
            fp.SetPreference("extensions.firebug.console.enableSites", true);
            fp.SetPreference("extensions.firebug.net.enableSites", true);
            fp.SetPreference("extensions.firebug.script.enableSites", true);
            fp.SetPreference("extensions.firebug.allPagesActivation", "on");
            fp.AddExtension(extensionFile);
            fp.SetPreference("extensions.firebug.currentVersion", "1.12.5"); // Avoid startup firebug page on separate tab
            IWebDriver driver = new FirefoxDriver(fp);
            driver.Navigate().GoToUrl("http://testwisely.com/demo");
            // firefox started with firebug, however, unable to activate it.
            driver.Quit();
        }
    }

}