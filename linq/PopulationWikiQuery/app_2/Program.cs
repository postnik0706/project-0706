using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace app_2
{
    class Province
    {
        public int ID { get; set; }
        public string Town { get; set; }
        public string Name { get; set; }
        public string Population { get; set; }

        static List<Province> SampleData()
        {
            return new List<Province>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*
            string addr = "http://en.wikipedia.org/wiki/Canada";
            HtmlAgilityPack.HtmlWeb w = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = w.Load(addr);
            
            var result = doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[1]")
                .AsEnumerable()
                .Select((province, index) => new
                    {
                        province.InnerText,
                        i = index
                    })
                .Join(
                    doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[2]")
                    .AsEnumerable()
                    .Select((town, index) => new
                    {
                        town.InnerText,
                        i = index
                    }), id => id.i, id => id.i, (province, town) => new { Name = province.InnerText, Town = town.InnerText })
                ;
            
            foreach (var i in result)
            {
                Console.WriteLine("{0}  {1}", i.Name, i.Town);
            }
            */

            foreach (var t in Enumerable.Range('A', 'Z'-'A').OfType<Char>())
                Console.WriteLine(t);

            /*
            (from a in doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[1]")
                select new Province { Name = a.InnerText, ID = i++ })
            .Join(
                from a in doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[2]")
                select new Province { Town = a.InnerText, ID = i++ }, t => t.ID, t => t.ID,
            */
            /*
            int i = 0;
            var countries = from a in doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[1]")
                            select new Province { Name = a.InnerText, ID = i++ };
            i = 0;
            var towns = from a in doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[2]")
                        select new Province { Town = a.InnerText, ID = i++ };
            i = 0;
            var populations = from a in doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[3]")
                        select new Province { Population = a.InnerText, ID = i++ };

            countries.Join(

            foreach (var j in populations)
            {
                Console.WriteLine(j.Population);
            }
            */
            /*
            Console.WriteLine("--------------------");
            foreach (string item in doc.DocumentNode.SelectNodes(
                "/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[2]/a")
                .Select(t => t.InnerText)
                .ToArray())
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("--------------------");
            foreach (string item in doc.DocumentNode.SelectNodes(
                "/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[3]")
                .Select(t => t.InnerText)
                .ToArray())
            {
                Console.WriteLine(item);
            }
            */
            /*
            IEnumerable<Province> provinces =
                from townLine in doc.DocumentNode.SelectNodes(
                    "/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[1]/a")
                from provinceLine in doc.DocumentNode.SelectNodes(
                    "/html[1]/body[1]/div[3]/div[3]/div[4]/table[4]/tr/td[2]/a")
                //where townLine.LinePosition == provinceLine.LinePosition
                select new Province()
                {
                    Town = townLine.InnerText,
                    Name = provinceLine.InnerText
                };

            foreach (var item in provinces)
            {
                Console.WriteLine(item.Town + "\t" + item.Name);
            }
             */
        }
    }
}
