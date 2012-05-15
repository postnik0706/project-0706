using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace RandomDataGenerator
{
    class Program
    {
        private const string CACHE_FILENAME_LAST_NAMES = @"./dist.all.last";
        private const string CACHE_FILENAME_FIRST_NAMES = @"./dist.all.last";

        static void Main(string[] args)
        {
            // Wikipedia - 100 most popular last names
            /*
            HtmlWeb wiki = new HtmlWeb();
            HtmlDocument doc = wiki
                .Load("http://en.wikipedia.org/wiki/List_of_most_common_surnames_in_North_America");

            var tags = doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[1]");

            foreach (var item in tags)
            {
                Console.WriteLine(item.InnerText);
            }
            */
 
            // Census - most popular US names
            WebClient census = new WebClient();
            census.DownloadFile("http://www.census.gov/genealogy/www/data/1990surnames/dist.all.last", CACHE_FILENAME_LAST_NAMES);
            string[] names = (from n in File.ReadAllLines(CACHE_FILENAME_LAST_NAMES)
                              select n.Substring(0, 1).ToUpper() + (n.Substring(1, n.IndexOf(" ") - 1)).ToLower()).ToArray();
            census.DownloadFile("http://www.census.gov/genealogy/www/data/1990surnames/dist.female.first", CACHE_FILENAME
                              
                              
            foreach (var name in names)
            {
                Console.WriteLine(name);
            }
        }
    }
}
