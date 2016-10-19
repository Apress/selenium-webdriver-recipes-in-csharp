using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.IO;

public class GoogleSearchEdge  {
  public static void main(String[] args) {

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

    // And now use this to visit Google
    driver.Navigate().GoToUrl("http://www.google.com");

    // Find the text input element by its name
    IWebElement element = driver.FindElement(By.Name("q"));

    // Enter something to search for
    element.SendKeys("Hello Selenium WebDriver!");

    // Now submit the form. WebDriver will find the form for us from the element
    element.Submit();

    // Check the title of the page
    Console.WriteLine("Page title is: " + driver.Title);
	
	driver.Quit();
  }
}