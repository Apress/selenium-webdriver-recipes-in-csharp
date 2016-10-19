

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
    public class RadioButtonTest
    {

        static IWebDriver driver = new ChromeDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/radio_button.html");
        }

        [TestMethod]
        public void TestSelectRadioByNameAndValue()
        {
            driver.FindElement(By.XPath("//input[@name='gender' and @value='female']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//input[@name='gender' and @value='male']")).Click();
        }

        [TestMethod]
        public void TestSelectRadioOptionByID()
        {
            driver.FindElement(By.Id("radio_female")).Click();
            driver.FindElement(By.Id("radio_female")).Click(); // already selected, no effect
        }

        [TestMethod]
        public void TestClearRadioOption()
        {
            driver.FindElement(By.XPath("//input[@name='gender' and @value='female']")).Click();
            try
            {
                driver.FindElement(By.XPath("//input[@name='gender' and @value='female']")).Clear();
            }
            catch (Exception ex)
            {
                // Selenium does not allow
                Console.WriteLine("Selenium does not allow clear currently selected radio button, just select another one");
                driver.FindElement(By.XPath("//input[@name='gender' and @value='male']")).Click();
            }
        }

        [TestMethod]
        public void TestVerifyRadioOptionSelected()
        {
            driver.FindElement(By.XPath("//input[@name='gender' and @value='female']")).Click();
            Assert.IsTrue(driver.FindElement(By.XPath("//input[@name='gender' and @value='female']")).Selected);
            Assert.IsFalse(driver.FindElement(By.XPath("//input[@name='gender' and @value='male']")).Selected);
        }

        [TestMethod]
        public void TestIterateRadioOptions()
        {
            Assert.AreEqual(2, driver.FindElements(By.Name("gender")).Count);

            foreach (IWebElement rb in driver.FindElements(By.Name("gender")))
            {
                if (rb.GetAttribute("value").Equals("female"))
                {
                    rb.Click();
                }
            }

        }

        [TestMethod]
        public void TestClickNthRadioOption()
        {
            driver.FindElements(By.Name("gender"))[1].Click();
            Assert.IsTrue(driver.FindElement(By.XPath("//input[@name='gender' and @value='female']")).Selected);

            driver.FindElements(By.Name("gender"))[0].Click();
            Assert.IsTrue(driver.FindElement(By.XPath("//input[@name='gender' and @value='male']")).Selected);
        }

        [TestMethod]
        public void TestClickRadioOptionByTheFollowingLabel()
        {
            IWebElement elem  = driver.FindElement(By.XPath("//div[@id='q1']//label[contains(.,'Yes')]/../input[@type='radio']"));
            elem.Click();
        }

        [TestMethod]
        public void testCustomizeiCheckRadios(){
            // Error: Element is not clickable
            // driver.FindElement(By.Id("q2_1")).click();
            driver.FindElements(By.ClassName("iradio_square-red"))[0].Click();
            driver.FindElements(By.ClassName("iradio_square-red"))[1].Click();

            // More precise with XPath
            driver.FindElement(By.XPath("//div[contains(@class, 'iradio_square-red')]/input[@type='radio' and @value='male']/../ins")).Click();   
        }

        [TestCleanup]
        public void After()
        {
        }

        [ClassCleanup]
        public static void AfterAll()
        {
            // driver.Quit();
        }
    }

}