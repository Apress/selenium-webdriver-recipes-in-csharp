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
// requires Install Microsoft Office and make sure that the .NET Programmability Support feature is selected
// In Solution, Add References -> Assemblies -> Microsoft.Office.Interop.Excel
using Excel = Microsoft.Office.Interop.Excel;

namespace SeleniumRecipes.ch18_advanced
{
    [TestClass]
    public class DataDrivenExcelTest
    {

        static IWebDriver driver = new FirefoxDriver();

        /**
         *
         */
        [TestMethod]
        public void TestDataDrivenExcel()
        {
            String filePath = TestHelper.ScriptDir() + @"\testdata\users.xls";
            Console.WriteLine("Excel file: " + filePath);
            Excel.Application excelApp;
            Excel.Workbook excelWorkbook;
            Excel.Worksheet sheet;
            Excel.Range range;
            int rCnt = 0;
            int cCnt = 0;

            String description = null;
            String login = null;
            String password = null;
            String expectedText = null;

            excelApp = new Excel.Application();
            //Opening Excel file
            excelWorkbook = excelApp.Workbooks.Open(filePath);
            sheet = excelWorkbook.Worksheets.get_Item(1);

            range = sheet.UsedRange;

            // starting from 2, skip the header row
            for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)
            {
                Console.WriteLine("1.");

                Excel.Range myIDBinder = (Excel.Range)sheet.get_Range("A" + rCnt.ToString(), "A" + rCnt.ToString());
                description = myIDBinder.Value.ToString();

                myIDBinder = (Excel.Range)sheet.get_Range("B" + rCnt.ToString(), "B" + rCnt.ToString());
                login = myIDBinder.Value.ToString();

                myIDBinder = (Excel.Range)sheet.get_Range("C" + rCnt.ToString(), "C" + rCnt.ToString());
                password = myIDBinder.Value.ToString();

                myIDBinder = (Excel.Range)sheet.get_Range("D" + rCnt.ToString(), "D" + rCnt.ToString());
                expectedText = myIDBinder.Value.ToString();

                driver.Navigate().GoToUrl("http://travel.agileway.net");
                driver.FindElement(By.Name("username")).SendKeys(login);
                driver.FindElement(By.Name("password")).SendKeys(password);
                driver.FindElement(By.Name("username")).Submit();
                Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains(expectedText));

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

            excelWorkbook.Close(true, null, null);
            excelApp.Quit();
        }
    
    }
}
