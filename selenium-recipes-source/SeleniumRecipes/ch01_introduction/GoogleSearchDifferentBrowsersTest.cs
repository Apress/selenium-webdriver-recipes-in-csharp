using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System.Collections.ObjectModel;
using System.IO;


[TestClass]
public class GoogleSearchDifferentBrowsersTest {

    [TestMethod]
    public void TestInIE() {
            IWebDriver driver = new InternetExplorerDriver();
            driver.Navigate().GoToUrl("http://testwisely.com/demo");
            System.Threading.Thread.Sleep(1000);
            driver.Quit();
    }

    [TestMethod]
    public void TestInFirefox() {
        IWebDriver driver = new FirefoxDriver();
        driver.Navigate().GoToUrl("http://testwisely.com/demo");
        System.Threading.Thread.Sleep(1000);
        driver.Quit();
    }

    [TestMethod]
    public void TestInChrome() {
        IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl("http://testwisely.com/demo");
        System.Threading.Thread.Sleep(1000);
        driver.Quit();
    }


    [TestMethod]
    public void TestInEdge()
    {
        string serverPath = "Microsoft Web Driver";

        if (System.Environment.Is64BitOperatingSystem)
        {
            serverPath = Path.Combine(System.Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"), serverPath);
        }
        else
        {
            serverPath = Path.Combine(System.Environment.ExpandEnvironmentVariables("%ProgramFiles%"), serverPath);
        }

        // location for MicrosoftWebDriver.exe
        EdgeOptions options = new EdgeOptions();
        options.PageLoadStrategy = EdgePageLoadStrategy.Eager;
        IWebDriver driver = new EdgeDriver(serverPath, options);

        System.Diagnostics.Debug.Write("Start... ");

        driver.Navigate().GoToUrl("http://travel.agileway.net");
        driver.FindElement(By.Name("username")).SendKeys("agileway");
        IWebElement passwordElem =  driver.FindElement(By.Name("password"));
        passwordElem.SendKeys("testwise");
        passwordElem.Submit(); // not implemented 
        System.Threading.Thread.Sleep(1000);
        driver.Quit();
    }

}
