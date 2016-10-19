
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Data.SQLite;

namespace SeleniumRecipes.ch14_testdata
{
    [TestClass]
    public class QueryDatabaseTest
    {
        static IWebDriver driver = new FirefoxDriver();

        [TestMethod]
        public void TestDatabaseSqlite3()
        {
            driver.Navigate().GoToUrl(TestHelper.siteUrl().Replace("index.html", "text_field.html"));

            String oldestUserLogin = null;
            SQLiteConnection connection = null;

            try
            {
                String dbFile = TestHelper.scriptDir() + @"\testdata\sample.db";
                connection = new SQLiteConnection("Data Source=" + dbFile + ";Version=3");
                connection.Open();

                String sql = "select login from users order by age desc";
                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {    // read the result set
                    oldestUserLogin = (String)reader["login"];
                    break;
                }
            }
            catch (Exception e)
            {
                // probably means no database file is found
                Console.WriteLine(e.Message);
            }
            finally
            {
                try
                {
                    if (connection != null)
                    {
                        connection.Close();
                    }
                }
                catch (Exception e)
                {  // connection close failed.
                    Console.WriteLine(e);
                }
            }

            Console.WriteLine(" => " + oldestUserLogin);
            driver.FindElement(By.Id("user")).SendKeys(oldestUserLogin);
        }
    }
}
