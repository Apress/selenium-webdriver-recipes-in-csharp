using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumRecipes
{
    public class TestHelper
    {

        public static String SiteUrl() {
            // change to your installed location
            return "file:///C:/agileway/books/SeleniumRecipes-C%23/site";
        }

        // change to yours
        public static String ScriptDir()
        {
            return @"C:\agileway\books\SeleniumRecipes-C#\recipes";
        }

        public static String TempDir()
        {
            return "C:\\temp";
        }

    }
}
