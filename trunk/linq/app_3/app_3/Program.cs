using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using System.Globalization;

namespace app_3
{
    class Program
    {
        static void ShowCultures()
        {
            var output =
                from v in CultureInfo.GetCultures(CultureTypes.AllCultures)
                where v.Name.Contains("ru")
                select v;
            
            foreach (var item in output)
            {
                Console.WriteLine(item.Name);
            }
        }
        
        static void Main(string[] args)
        {
            //ShowCultures();
            
            CultureInfo info = CultureInfo.CreateSpecificCulture("ru-RU");
            info.DateTimeFormat.SetAllDateTimePatterns(
                new string[] { "dd.MM HH:mm" }, 'd');

            Encoding rusEncoding = Encoding.GetEncoding("windows-1251");
            Console.OutputEncoding = rusEncoding;
            WebClient w = new WebClient();
            w.Encoding = rusEncoding;
            string news = w.DownloadString(@"http://img.lenta.ru/r/EX/import.xml");
            
            XElement newsX = XElement.Parse(news);
            var output =
                from i in newsX.Elements("news")
                orderby DateTime.ParseExact(i.Element("date").Value, "d", info)
                select string.Format("{0}: {1}",
                    i.Element("date").Value, i.Element("title").Value);

            foreach (var item in output)
            {
                Console.WriteLine(item);
            }
        }
    }
}
