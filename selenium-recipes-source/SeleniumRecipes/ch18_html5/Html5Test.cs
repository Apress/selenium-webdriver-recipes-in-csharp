

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;

namespace SeleniumRecipes
{

    [TestClass]
    public class Html5Test
    {

        static IWebDriver driver = new ChromeDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/html5.html");
        }

        [TestMethod]
        public void TestEmailField()
        {
            driver.FindElement(By.Id("email")).SendKeys("test@wisely.com");
        }

        // only Chrome supports it at the moment
        [TestMethod]
        public void TestTimeField()
        {
            driver.FindElement(By.Id("start_time_1")).SendKeys("12:05AM");

            // focus on another ...
            driver.FindElement(By.Id("home_link")).SendKeys("");
            System.Threading.Thread.Sleep(500);

            // now back to change it
            driver.FindElement(By.Id("start_time_1")).Click();
            // [:delete, :left, :delete, :left, :delete]
            driver.FindElement(By.Id("start_time_1")).SendKeys(Keys.Delete);
            driver.FindElement(By.Id("start_time_1")).SendKeys(Keys.Left);
            driver.FindElement(By.Id("start_time_1")).SendKeys(Keys.Delete);
            driver.FindElement(By.Id("start_time_1")).SendKeys(Keys.Left);
            driver.FindElement(By.Id("start_time_1")).SendKeys(Keys.Delete);

            driver.FindElement(By.Id("start_time_1")).SendKeys("08");
            System.Threading.Thread.Sleep(300);
            driver.FindElement(By.Id("start_time_1")).SendKeys("27");
            System.Threading.Thread.Sleep(300);
            driver.FindElement(By.Id("start_time_1")).SendKeys("AM");
        }



        [TestMethod]
        public void TestOnClickForTextField()
        {
            driver.FindElement(By.Name("person_name")).Clear();
            driver.FindElement(By.Name("person_name")).SendKeys("Wise Tester");

            driver.FindElement(By.Name("person_name")).Click();
            driver.FindElement(By.Id("tip")).Text.Equals("Max 20 characters");
        }

        [TestMethod]
        public void TestOnChangeForTextField()
        {
            driver.FindElement(By.Name("person_name")).Clear();
            driver.FindElement(By.Name("person_name")).SendKeys("Wise Tester too");
            ((IJavaScriptExecutor)driver).ExecuteScript("$('#person_name_textbox').trigger('change')");
            Assert.IsTrue(driver.FindElement(By.Id("person_name_label")).Text.Equals("Wise Tester too"));
        }



        [TestMethod]
        public void TestChosenSingle() {
         driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/chosen/index.html");

        System.Threading.Thread.Sleep(2000);  // wait enough time to load JS
        driver.FindElement(By.XPath("//div[@id='chosen_single_chosen']//a[contains(@class,'chosen-single')]")).Click();
        ReadOnlyCollection<IWebElement> available_items = driver.FindElements(By.XPath("//div[@id='chosen_single_chosen']//div[@class='chosen-drop']//li[contains(@class,'active-result')]"));
        foreach (IWebElement item in available_items)
        {
            if (item.Text.Equals("Australia")) {
                item.Click();
                break;
            }
        }
        System.Threading.Thread.Sleep(1000);

        driver.FindElement(By.XPath("//div[@id='chosen_single_chosen']//a[contains(@class,'chosen-single')]")).Click();
        available_items = driver.FindElements(By.XPath("//div[@id='chosen_single_chosen']//div[@class='chosen-drop']//li[contains(@class,'active-result')]"));
        foreach (IWebElement item in available_items)
	    {
		    if (item.Text.Equals("United States")) {
                item.Click();
                break;
            } 
	    }
     
        // Now search for an option
        System.Threading.Thread.Sleep(1000);
        driver.FindElement(By.XPath("//div[@id='chosen_single_chosen']//a[contains(@class,'chosen-single')]")).Click();

        IWebElement search_text_field = driver.FindElement(By.XPath("//div[@id='chosen_single_chosen']//div[@class='chosen-drop']//div[contains(@class,'chosen-search')]/input"));
        search_text_field.SendKeys("United King");
        System.Threading.Thread.Sleep(500); // let filtering finishing
        // select first selected option
        search_text_field.SendKeys(Keys.Enter);
    }

        [TestMethod]
        public void TestChosenMultiple() {
        driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/chosen/index.html");

        System.Threading.Thread.Sleep(2000);  // wait enough time to load JS
        driver.FindElement(By.XPath("//div[@id='chosen_multiple_chosen']//li[@class='search-field']/input")).Click();
        ReadOnlyCollection<IWebElement> available_items = driver.FindElements(By.XPath("//div[@id='chosen_multiple_chosen']//div[@class='chosen-drop']//li[contains(@class,'active-result')]"));
        
       foreach (IWebElement item in available_items) {
            if (item.Text.Equals("Australia")) {
                item.Click();
                break;
            }
        }
        System.Threading.Thread.Sleep(1000);

        // select another
        driver.FindElement(By.XPath("//div[@id='chosen_multiple_chosen']//li[@class='search-field']/input")).Click();
        available_items = driver.FindElements(By.XPath("//div[@id='chosen_multiple_chosen']//div[@class='chosen-drop']//li[contains(@class,'active-result')]"));
       foreach (IWebElement item in available_items) {
            if (item.Text.Equals("United Kingdom")) {
                item.Click();
                break;
            }
        }

        System.Threading.Thread.Sleep(500);
        // clear all selections
        ReadOnlyCollection<IWebElement> closeButtons = driver.FindElements(By.XPath("//div[@id='chosen_multiple_chosen']//ul[@class='chosen-choices']/li[contains(@class,'search-choice')]/a[contains(@class,'search-choice-close')]"));
               foreach (IWebElement item in closeButtons) {
            item.Click();
        }

        // then select one after clear
        driver.FindElement(By.XPath("//div[@id='chosen_multiple_chosen']//li[@class='search-field']/input")).Click();
        available_items = driver.FindElements(By.XPath("//div[@id='chosen_multiple_chosen']//div[@class='chosen-drop']//li[contains(@class,'active-result')]"));
        foreach (IWebElement item in available_items)
        {
            if (item.Text.Equals("United States"))
            {
                item.Click();
                break;
            }
        }

    }

        public void ClearChosen(String elemId) {
        System.Threading.Thread.Sleep(500);
        ReadOnlyCollection<IWebElement> closeButtons = driver.FindElements(By.XPath("//div[@id='" + elemId + "']//ul[@class='chosen-choices']/li[contains(@class,'search-choice')]/a[contains(@class,'search-choice-close')]"));
        foreach (IWebElement closeButton in closeButtons)
        {
            closeButton.Click();
        }
    }

        public void SelectChosenByLabel(String elemId, String label) {
        driver.FindElement(By.XPath("//div[@id='" + elemId + "']//li[@class='search-field']/input")).Click();
        ReadOnlyCollection<IWebElement> availableItems = driver.FindElements(By.XPath("//div[@id='" + elemId + "']//div[@class='chosen-drop']//li[contains(@class,'active-result')]"));
        foreach (IWebElement item in availableItems)
        {
            if (item.Text.Equals(label)) {
                item.Click();
                break;
            }
        }
    }

        [TestMethod]
        public void TestChosenMultipleCallingMethods()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/chosen/index.html");
            // ... land to the page with a chosen select list
            ClearChosen("chosen_multiple_chosen");
            SelectChosenByLabel("chosen_multiple_chosen", "United States");
            SelectChosenByLabel("chosen_multiple_chosen", "Australia");
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