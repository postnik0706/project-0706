using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace RandomDataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web
                .Load("http://en.wikipedia.org/wiki/List_of_most_common_surnames_in_North_America");

            var tags = doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[1]");

            foreach (var item in tags)
            {
                Console.WriteLine(item.InnerText);
            }
        }
    }
}
