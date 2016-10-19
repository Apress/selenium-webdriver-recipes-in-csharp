using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;
using System.IO;
using System.Collections.Generic;
using OpenQA.Selenium.Interactions;

namespace SeleniumRecipes
{

    [TestClass]
    public class RichTextEditorTest
    {

        static IWebDriver driver;
        
        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
            driver = new ChromeDriver();
        }

        [TestInitialize]
        public void Before()
        {
        }

        [TestMethod]
        public void TestTinyMCE()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/tinymce-4.1.9/tinyice_demo.html");

            IWebElement tinymceFrame = driver.FindElement(By.Id("mce_0_ifr"));
            driver.SwitchTo().Frame(tinymceFrame);
            IWebElement editorBody = driver.FindElement(By.CssSelector("body"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].innerHTML = '<h1>Heading</h1>AgileWay'", editorBody);
            System.Threading.Thread.Sleep(500);
            editorBody.SendKeys("New content");
            System.Threading.Thread.Sleep(500);
            editorBody.Clear();

            // click TinyMCE editor's 'Numbered List' button
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].innerHTML = '<p>one</p><p>two</p>'", editorBody);

            // switch out then can drive controls on the main page
            driver.SwitchTo().DefaultContent();
            IWebElement tinymceNumberListBtn = driver.FindElement(By.CssSelector(".mce-btn[aria-label='Numbered list'] button"));
            tinymceNumberListBtn.Click();

            // Insert using JavaScripts
            ((IJavaScriptExecutor)driver).ExecuteScript("tinyMCE.activeEditor.insertContent('<p>Brisbane</p>')");
        }

        [TestMethod]
        public void TestCKEditor()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/ckeditor-4.4.7/samples/uicolor.html");
            System.Threading.Thread.Sleep(500);
            IWebElement ckeditorFrame = driver.FindElement(By.ClassName("cke_wysiwyg_frame"));
            driver.SwitchTo().Frame(ckeditorFrame);
            IWebElement editorBody = driver.FindElement(By.TagName("body"));
            editorBody.SendKeys("Selenium Recipes\n by Zhimin Zhan");
            System.Threading.Thread.Sleep(500);
            editorBody.SendKeys("New content");
            System.Threading.Thread.Sleep(500);
            editorBody.Clear();

            // Clear content Another Method Using ActionBuilder to clear()        
            Actions builder = new Actions(driver);
            builder.Click(editorBody)
                    .KeyDown(Keys.Control)
                    .SendKeys("a")
                    .KeyUp(Keys.Control)
                    .Perform();
            builder.SendKeys(Keys.Backspace)
                    .Perform();

            // switch out then can drive controls on the main page
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.ClassName("cke_button__numberedlist")).Click(); // numbered list
        }


        [TestMethod]
        public void TestSummerNote()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/summernote-0.6.3/demo.html");
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//div[@class='note-editor']/div[@class='note-editable']")).SendKeys("Text");
            // click a format button: unordered list
            driver.FindElement(By.XPath("//button[@data-event='insertUnorderedList']")).Click();
            // switch to code view
            driver.FindElement(By.XPath("//button[@data-event='codeview']")).Click();
            // insert code (unformatted)
            driver.FindElement(By.XPath("//textarea[@class='note-codable']")).SendKeys("\n<p>HTML</p>");
        }

        [TestMethod]
        public void TestCodeMirror()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/codemirror-5.1/demo/xmlcomplete.html");
            System.Threading.Thread.Sleep(500);
            IWebElement elem = driver.FindElement(By.ClassName("CodeMirror-scroll"));
            elem.Click();
            System.Threading.Thread.Sleep(500);
            // elem.sendKeys does not work
            Actions builder = new Actions(driver);
            builder.SendKeys("<h3>Heading 3</h3><p>TestWise is Selenium IDE</p>")
                    .Perform();
        }

        [TestCleanup]
        public void After()
        {
        }

        [ClassCleanup]
        public static void AfterAll()
        {
            if (driver != null)
            {
               driver.Quit();
            }
        }

    }
}
