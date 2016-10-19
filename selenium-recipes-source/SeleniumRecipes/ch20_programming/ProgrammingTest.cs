
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
using System.Text.RegularExpressions;

namespace SeleniumRecipes
{

    [TestClass]
    public class AdvancedTest
    {

        static IWebDriver driver = new ChromeDriver();

        String site_root_url;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/select_list.html");
        }


        [TestMethod]
        public void TestThrowNewExceptions()
        {
            string osPlatform = System.Environment.OSVersion.Platform.ToString();
            System.Console.WriteLine(osPlatform);
            if (osPlatform != "Win32NT3")
            {
                throw new Exception("Unsupported platform: " + osPlatform);
            }

            try
            {
                throw new Exception("To be done");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured:  " + ex + ", " + ex.StackTrace + ". but it is OK to ignore");
            }
            finally
            {
                Console.WriteLine("Always called");
            }
            // ... 
        }


        [TestMethod]
        public void TestIgnorableError()
        {
            try
            {
                driver.FindElement(By.Name("notExists")).Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured:  " + ex + ", but it is OK to ignore");
            }
            // ... 
        }

        [TestMethod]
        public void TestReadExtnernalFile()
        {
            String filePath = TestHelper.ScriptDir() + @"\testdata\in.xml";
            Console.WriteLine(filePath);
            Assert.IsTrue(File.Exists(filePath));
            String content = File.ReadAllText(filePath);
            Console.WriteLine("content = " + content);
        }



        /**
         * Using OpenCSV library
         */
        [TestMethod]
        public void TestDataDrivenCSV()
        {
            // Iterate each row in the CSV file, use data for test scripts
            String csvFilePath = TestHelper.ScriptDir() + @"\testdata\users.csv";

            IEnumerable<IList<string>> data = CSVReader.FromFile(csvFilePath);

            foreach (IList<string> nextLine in data)
            {
                // nextLine[] is an array of values from the line
                String login = nextLine[1];
                String password = nextLine[2];
                String expected_text = nextLine[3];

                if (login.Equals("LOGIN"))
                { // head row
                    continue;
                }
                driver.Navigate().GoToUrl("http://travel.agileway.net");
                driver.FindElement(By.Name("username")).SendKeys(login);
                driver.FindElement(By.Name("password")).SendKeys(password);
                driver.FindElement(By.Name("username")).Submit();
                Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains(expected_text));

                try
                {
                    // if logged in OK, try log out, so next one can continue
                    driver.FindElement(By.LinkText("Sign off")).Click();
                }
                catch (Exception ex)
                {
                    // ignore
                }
            }

        }

        [TestMethod]
        public void TestSendSpecialKeys()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/text_field.html");
            IWebElement elem = driver.FindElement(By.Id("user"));
            elem.Clear();
            elem.SendKeys("agileway");
            System.Threading.Thread.Sleep(1000); // sleep for seeing the effect

            // select all (Ctrl+A) then press backspace
            elem.SendKeys(Keys.Control + "A");
            elem.SendKeys(Keys.Backspace);
            System.Threading.Thread.Sleep(1000);
            elem.SendKeys("testwisely");
            System.Threading.Thread.Sleep(1000);
            elem.SendKeys(Keys.Enter);  //submit the form
        }

        [TestMethod]
        public void TestEnterOrCheckUnicode()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/assert.html");
            Assert.AreEqual("空気", driver.FindElement(By.Id("unicode_test")).Text);
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/text_field.html");
            driver.FindElement(By.Id("user")).SendKeys("проворный");
        }

        [TestMethod]
        public void TestIgnoreDynamicallyGeneratePrefix()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/text_field.html");
            IWebElement elemByName = driver.FindElement(By.Name("ctl00$m$g_dcb0d043_e7f0_4128_99c6_71c113f45dd8$ctl00$tAppName"));
            elemByName.SendKeys("full name");
            elemByName.Clear();
            System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.XPath("//input[contains(@name, 'AppName')]")).SendKeys("I still can");
        }

        [TestMethod]
        public void TestVerifyPageTextAndSourceAndHtmlEntities()
        {
            driver.Navigate().GoToUrl("http://testwisely.com/demo/assertion");
            // tags in source not in text
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("BOLD Italic"));
            Assert.IsTrue(driver.PageSource.Contains("<b>BOLD</b>  <i>Italic</i>"));
            // HTML entities in source but shown as space in text
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("assertion  \r\n(new line before)"));
            // note the second character after assertion is non-breaable space (&nbsp;)

            String browserName = ((RemoteWebDriver)driver).Capabilities.BrowserName;
            if (browserName.Equals("firefox"))
            {
                // different behaviour on Firefox
                Assert.IsTrue(driver.PageSource.Contains("assertion  \r\n(new line before)"));
            }
            else if (browserName.Equals("chrome"))
            {
                Assert.IsTrue(driver.PageSource.Contains("assertion  \r\n(new line before)"));
            }
            else 
            {
                Assert.IsTrue(driver.PageSource.Contains("assertion &nbsp;\r\n(new line before)"));
            }
        }



        [TestMethod]
        public void TestScrollDownToPageBottom()
        {
            driver.Navigate().GoToUrl("http://clinicwise.net/pricing");
            System.Threading.Thread.Sleep(500);

            // JavaScript API
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

            // Or

            // Send Keyboard command
            driver.FindElement(By.TagName("body")).SendKeys(Keys.Control + Keys.End);
        }

        [TestMethod]
        public void TestVerifySearchResultsOrder()  {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/data_grid.html");
            System.Threading.Thread.Sleep(500); // wait JS to load
            driver.FindElement(By.Id("heading_product")).Click(); // sort asc                
            ReadOnlyCollection<IWebElement> firstCells = driver.FindElements(By.XPath("//tbody/tr/td[1]"));
	        List<String> productNames = new List<String>();
            foreach (IWebElement elem in firstCells) {;  productNames.Add(elem.Text); }      
            List<String> sortedProductNames =  new List<String>(productNames);
            sortedProductNames.Sort();
            Console.WriteLine(productNames);
            Console.WriteLine(sortedProductNames);
            Assert.AreEqual(productNames.ToString(), sortedProductNames.ToString());

            driver.FindElement(By.Id("heading_product")).Click(); // sort desc                
            System.Threading.Thread.Sleep(500);
            firstCells = driver.FindElements(By.XPath("//tbody/tr/td[1]"));
            productNames = new List<String>();
            foreach (IWebElement elem in firstCells) { ;  productNames.Add(elem.Text); }
            sortedProductNames = new List<String>(productNames);
            sortedProductNames.Sort();
            sortedProductNames.Reverse();
            Assert.AreEqual(productNames.ToString(), sortedProductNames.ToString());
        }
    

        [TestMethod]
        public void TestVerifyDataUniqueness() {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/data_grid.html");
            System.Threading.Thread.Sleep(500); // wait JS to load
            ReadOnlyCollection<IWebElement> secondCells = driver.FindElements(By.XPath("//tbody/tr/td[2]"));
            List<String> yearsReleased = new List<String>();
            foreach (IWebElement elem in secondCells) {;  yearsReleased.Add(elem.Text); }
            Assert.AreEqual(yearsReleased.Count, new HashSet<String>(yearsReleased).Count);
        }


        [TestMethod]
        public void TestResponsiveCheckControlWidth()
        {
            driver.Manage().Window.Size = new System.Drawing.Size(1024, 768); // Desktop
            driver.Url = "https://support.agileway.net";
            int widthDesktop = driver.FindElement(By.Name("email")).Size.Width;
            Console.WriteLine(widthDesktop);
            driver.Manage().Window.Size = new System.Drawing.Size(768, 1024); // iPad
            int widthIPad = driver.FindElement(By.Name("email")).Size.Width;
            Console.WriteLine(widthIPad); // 358 vs 960
            Assert.IsTrue(widthDesktop < widthIPad);
        }

        [TestMethod]
        public void TestTableWithRowsDynamiicalySetHidden()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/data_grid.html");
            driver.Manage().Window.Size = new System.Drawing.Size(1024, 768);

            ReadOnlyCollection<IWebElement> rows = driver.FindElements(By.XPath("//table[@id='grid']/tbody/tr"));
            Assert.AreEqual(4, rows.Count);
            String firstProductName = driver.FindElement(By.XPath("//table[@id='grid']//tbody/tr[1]/td[1]")).Text;
            Assert.AreEqual("ClinicWise", firstProductName);
            driver.FindElement(By.XPath("//table[@id='grid']//tbody/tr[1]/td/button")).Click();

            driver.FindElement(By.Id("test_products_only_flag")).Click();
            System.Threading.Thread.Sleep(100);
            // Element is not currently visible
            // driver.FindElement(By.XPath("//table[@id='grid']//tbody/tr[1]/td/button")).Click();

            ReadOnlyCollection<IWebElement>  displayedRows = driver.FindElements(By.XPath("//table[@id='grid']//tbody/tr[not(contains(@style,'display: none'))]"));
            Assert.AreEqual(2, displayedRows.Count);
            IWebElement firstRowElem = displayedRows[0];

            String newFirstProductName = firstRowElem.FindElement(By.XPath("td[1]")).Text;
            Assert.AreEqual("BuildWise", newFirstProductName);
            firstRowElem.FindElement(By.XPath("td/button")).Click();
        }

        [TestMethod]
        public void TestExtractDynamicTextUsingRegex()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/coupon.html");
            driver.FindElement(By.Id("get_coupon_btn")).Click();
            var couponText = driver.FindElement(By.Id("details")).Text;
            System.Console.WriteLine(couponText);
            Match match = Regex.Match(couponText, @"coupon code:\s+(\w+) used by\s([\d|-]+)");
            if (match.Success)
            {
                string coupon = match.Groups[1].Value;
                string expiryDate = match.Groups[2].Value;
                // System.Console.WriteLine(expiryDate);
                // System.Console.WriteLine(coupon);
                driver.FindElement(By.Name("coupon")).SendKeys(coupon);
            }
            else {
                throw new Exception("Error: no valid coupon returned");
            }
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