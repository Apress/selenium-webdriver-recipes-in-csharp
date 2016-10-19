using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;

namespace SeleniumRecipes.ch17_advanced_user_interaction
{
    [TestClass]
    public class AdvancedUserInteractionTest
    {

        static IWebDriver driver = new ChromeDriver();

        String siteRootUrl;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/html5.html");
        }


        [TestMethod]
        public void TestDoubleClick()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/text_field.html");
            IWebElement elem = driver.FindElement(By.Id("pass"));
            Actions builder = new Actions(driver);
            builder.DoubleClick(elem).Perform();
        }

        [TestMethod]
        public void TestMouseOver()
        {
            IWebElement elem = driver.FindElement(By.Id("email"));
            Actions builder = new Actions(driver);
            builder.MoveToElement(elem).Perform();
        }

        [TestMethod]
        public void TestClickAndHold()
        {
            driver.Navigate().GoToUrl("http://jqueryui.com/selectable");
            driver.FindElement(By.LinkText("Display as grid")).Click();
            System.Threading.Thread.Sleep(500);
            driver.SwitchTo().Frame(0);
            ReadOnlyCollection<IWebElement> listItems = driver.FindElements(By.XPath("//ol[@id='selectable']/li"));
            Actions builder = new Actions(driver);
            builder.ClickAndHold(listItems[1])
                   .ClickAndHold(listItems[3])
                   .Click()
                   .Perform();
            driver.SwitchTo().DefaultContent();
        }

        [TestMethod]
        public void TestRightClick()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/text_field.html");
            System.Threading.Thread.Sleep(500);
            IWebElement elem = driver.FindElement(By.Id("pass"));

            ICapabilities caps = ((RemoteWebDriver)driver).Capabilities;
            if (caps.BrowserName == "firefox")
            {
                Actions builder = new Actions(driver);
                builder.ContextClick(elem)
                       .SendKeys(Keys.Down)
                       .SendKeys(Keys.Down)
                       .SendKeys(Keys.Down)
                       .SendKeys(Keys.Down)
                       .SendKeys(Keys.Return)
                       .Perform();
            }
            else
            {
                // ...
            }
        }

        public void TestKeySequenceSelectAllAndDelete()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/text_field.html");
            driver.FindElement(By.Id("comments")).SendKeys("Multiple Line\r\n Text");
            IWebElement elem = driver.FindElement(By.Id("comments"));

            Actions builder = new Actions(driver);
            builder.Click(elem)
                   .KeyDown(Keys.Control)
                   .SendKeys("a")
                   .KeyUp(Keys.Control)
                   .Perform();
            // this different from click element, the key is send to browser directly
            builder = new Actions(driver);
            builder.SendKeys(Keys.Backspace).Perform();
        }

        [TestMethod]
        public void TestDragAndDrop()
        {
            //this works OK on Chrome, error on Firefox, IE no effect
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/drag_n_drop.html");
            IWebElement dragFrom = driver.FindElement(By.Id("item_1"));
            IWebElement target = driver.FindElement(By.Id("trash"));

            Actions builder = new Actions(driver);
            IAction dragAndDrop = builder.ClickAndHold(dragFrom)
                    .MoveToElement(target)
                    .Release(target).Build();

            dragAndDrop.Perform();
        }

        [TestMethod]
        public void TestSlider()
        {
            // this does not working on Firefox, yet
            Assert.AreEqual("15%", driver.FindElement(By.Id("pass_rate")).Text);
            IWebElement elem = driver.FindElement(By.Id("pass-rate-slider"));

            Actions move = new Actions(driver);
            move.DragAndDropToOffset(elem, 30, 0).Perform();
            Assert.AreNotEqual("15%", driver.FindElement(By.Id("pass_rate")).Text);
        }


    }
}
