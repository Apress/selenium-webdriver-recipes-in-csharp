
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

namespace SeleniumRecipes
{

    [TestClass]
    public class OptimizationDynamicTest
    {

        static IWebDriver driver;
        static String siteRootUrl = "http://physio.clinicwise.net";
        // static String siteRootUrl = "http://yake.clinicwise.net";

        private static readonly Dictionary<string, string> natalieUserDict = new Dictionary<string, string> {
            { "english", "natalie" },
            { "chinese", "tuo" },
            { "french", "dupont" }
        };

        private static readonly Dictionary<string, string> markUserDict = new Dictionary<string, string> {
            { "english", "mark" },
            { "chinese", "li" },
            { "french", "marc" }
        };

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
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
        public void TestUseEnvironmentVariableToChangeTestBehaviourDynamically()
        {
            // Commment and 
            // driver.Navigate().GoToUrl("http://yake.clinicwise.net");
            // driver.Navigate().GoToUrl("http://phsyio.clinicwise.net");

            // Warning: Visual Studio Caching Environement Variables

            String browserTypeSetInEnv = Environment.GetEnvironmentVariable("BROWSER");
            Console.WriteLine(browserTypeSetInEnv);
            if (!String.IsNullOrEmpty(browserTypeSetInEnv) && browserTypeSetInEnv.Equals("Chrome"))
            {
                driver = new ChromeDriver();
            }
            else
            {
                driver = new FirefoxDriver();
            }

            if (!String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("SITE_URL")))
            {
                siteRootUrl = Environment.GetEnvironmentVariable("SITE_URL");
            }
            driver.Navigate().GoToUrl(siteRootUrl);
        }


        [TestMethod]
        public void TestTwoLanguagesUsingIfElese()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(siteRootUrl); // may be dynamically set

            if (siteRootUrl.Contains("physio.clinicwise.net"))
            {
                driver.FindElement(By.Id("username")).SendKeys("natalie");
                driver.FindElement(By.Id("password")).SendKeys("test");
                driver.FindElement(By.Id("signin_button")).Click();
                Assert.IsTrue(driver.PageSource.Contains("Signed in successfully."));
            }
            else if (siteRootUrl.Contains("yake.clinicwise.net"))
            {
                driver.FindElement(By.Id("username")).SendKeys("tuo");
                driver.FindElement(By.Id("password")).SendKeys("test");
                driver.FindElement(By.Id("signin_button")).Click();
                Assert.IsTrue(driver.PageSource.Contains("成功登录"));
            }

        }


        public bool IsChinese()
        {
            return siteRootUrl.Contains("yake.clinicwise.net");
        }

        [TestMethod]
        public void TestTwoLanguagesUsingTernaryOperator()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(siteRootUrl); // may be dynamically set

            driver.FindElement(By.Id("username")).SendKeys(IsChinese() ? "tuo" : "natalie");
            driver.FindElement(By.Id("password")).SendKeys("test");
            driver.FindElement(By.Id("signin_button")).Click();
            Assert.IsTrue(driver.PageSource.Contains(IsChinese() ? "成功登录" : "Signed in successfully."));
        }

        public string SiteLang()
        {
            if (siteRootUrl.Contains("yake.clinicwise.net"))
            {
                return "chinese";
            } else if (siteRootUrl.Contains("sandbox.clinicwise.net"))
            {
                return "french";
            } else
            {
                return "english";
            }
        }


        [TestMethod]
        public void TestMultipleLanguagesUsingIfElse()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(siteRootUrl); // may be dynamically set

            if (SiteLang() == "chinese")
            {
                driver.FindElement(By.Id("username")).SendKeys("tuo");
            }
            else if (SiteLang() == "french")
            {
                driver.FindElement(By.Id("username")).SendKeys("dupont");
            }
            else { // default
                driver.FindElement(By.Id("username")).SendKeys("natalie");
            }
            driver.FindElement(By.Id("password")).SendKeys("test");
            driver.FindElement(By.Id("signin_button")).Click();
        }

        public String UserLookup(string username)
        {
            switch (SiteLang())
            {
                case "chinese":
                    return "tuo";
                  
                case "french":
                    return "dupont";

                default:
                    return username;
            }
        }

        [TestMethod]
        public void TestMultipleLanguagesUsingLookup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(siteRootUrl); // may be dynamically set

            driver.FindElement(By.Id("username")).SendKeys(UserLookup("natalie"));
            driver.FindElement(By.Id("password")).SendKeys("test");
            driver.FindElement(By.Id("signin_button")).Click();
        }


        public String UserLookupDict(string username)
        {
            switch (username)
            {
                case "natalie":
                    return natalieUserDict[SiteLang()];

                case "mark":
                    return markUserDict[SiteLang()];

                default:
                    return username;
            }
        }

        [TestMethod]
        public void TestMultipleLanguagesUsingHashLookup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(siteRootUrl); // may be dynamically set
            driver.FindElement(By.Id("username")).SendKeys(UserLookupDict("natalie"));
            driver.FindElement(By.Id("password")).SendKeys("test");
            driver.FindElement(By.Id("signin_button")).Click();
        }



    }
}