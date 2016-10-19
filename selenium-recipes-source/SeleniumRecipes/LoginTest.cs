using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;

namespace SeleniumRecipes
{
    [TestClass]
    public class LoginTest
    {

        IWebDriver driver;

        [TestMethod]
        public void TestLoginInFirefox()
        {
            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl("http://travel.agileway.net");
            driver.FindElement(By.Name("username")).SendKeys("agileway");
            driver.FindElement(By.Name("password")).SendKeys("testwise");
            driver.FindElement(By.Name("password")).Submit();
            Assert.IsTrue(driver.PageSource.Contains("Signed in!"));
            driver.Quit();
        }

        [TestMethod]
        public void TestLoginInIE()
        {
            driver = new InternetExplorerDriver();
            driver.Navigate().GoToUrl("http://travel.agileway.net");
            driver.FindElement(By.Name("username")).SendKeys("agileway");
            driver.FindElement(By.Name("password")).SendKeys("testwise");
            driver.FindElement(By.Name("password")).Submit();
            Assert.IsTrue(driver.PageSource.Contains("Signed in!"));
            driver.Quit();
        }


        [TestMethod]
        public void TestLoginInChrome()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://travel.agileway.net");
            driver.FindElement(By.Name("username")).SendKeys("agileway");
            driver.FindElement(By.Name("password")).SendKeys("testwise");
            driver.FindElement(By.Name("password")).Submit();
            Assert.IsTrue(driver.PageSource.Contains("Signed in!"));
            driver.Quit();
        }


    }
}
