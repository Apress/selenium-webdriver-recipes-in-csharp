
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;

namespace SeleniumRecipes
{

    [TestClass]
    public class TestDataTest
    {

        static IWebDriver driver = new FirefoxDriver();
        static Random _random; 

        String site_root_url;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
            _random = new Random();
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/index.html");
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
        public void TestDynamicDate()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/text_field.html");
            String todaysDate = DateTime.Now.ToString("MM/dd/yyyy");
            Console.WriteLine("todaysDate = " + todaysDate);
            driver.FindElement(By.Name("username")).SendKeys(todaysDate);

            driver.FindElement(By.Name("username")).SendKeys(Today("dd/MM/yyyy"));
            driver.FindElement(By.Name("username")).SendKeys(Tomorrow("AUS"));
            driver.FindElement(By.Name("username")).SendKeys(Yesterday("ISO"));
        }

        public static String GetDate(String format, int dateDiff)
        {
            if (format == null)
            {
                format = "MM/dd/yyyy";
            }
            else if (format.Equals("AUS") || format.Equals("UK"))
            {
                format = "dd/MM/yyyy";
            }
            else if (format.Equals("ISO"))
            {
                format = "yyyy-MM-dd";
            }

            DateTime today = DateTime.Today;
            return today.AddDays(dateDiff).ToString(format);    
        }

        public static String Today(String format)
        {
            return GetDate(format, 0);
        }

        public static String Tomorrow(String format)
        {
            return GetDate(format, 1);
        }

        public static String Yesterday(String format)
        {
            return GetDate(format, -1);
        }

        [TestMethod]
        public void TestRandomBooleanForRadio()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/radio_button.html");
            String randomGender = GetRandomBoolean() ? "male" : "female";
            driver.FindElement(By.XPath("//input[@type='radio' and @name='gender' and @value='" + randomGender + "']")).Click();
        }

        [TestMethod]
        public void TestRandomStringForTextField()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/text_field.html");
            driver.FindElement(By.Name("password")).SendKeys(GetRandomString(8));
        }

        [TestMethod]
        public void TestRandomNumberForTextField()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/text_field.html");
            // a number between 10 and 99, will be different each run   
            driver.FindElement(By.Name("username")).SendKeys("tester" + GetRandomNumber(10, 99));
        }

        [TestMethod]
        public void TestRandomStringInCollection()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/text_field.html");
            String[] allowableStrings = new String[] { "Yes", "No", "Maybe" }; // one of these strings
            driver.FindElement(By.Name("username")).SendKeys(GetRandomStringIn(allowableStrings));
        }

        [TestMethod]
        public void TestGenerateFixedSizeFile()
        {
            String outputFilePath = TestHelper.TempDir() + @"\2MB.txt";
            File.WriteAllBytes( outputFilePath,  new byte[1024 * 1024 * 2]);
        }

        public static Boolean GetRandomBoolean()
        {
            int random_0_or_1 = _random.Next(0, 2);
            return random_0_or_1 > 0 ? true : false;
        }

        public static int GetRandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        public static char GetRandomChar()
        {
            int num = _random.Next(0, 26); // Zero to 25
            char let = (char)('A' + num);
            return let;
        }

        public static String GetRandomString(int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(GetRandomChar());
            }
            return sb.ToString();
        }

        public static String GetRandomStringIn(String[] array)
        {
            return array[GetRandomNumber(0, array.Length - 1)];
        }


    }


}