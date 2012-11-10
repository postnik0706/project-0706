using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using System.Web;

namespace app_1
{
    public class Program
    {
        public static string[] GetAlphabet()
        {
            return Enumerable.Range('A', 'Z' - 'A' + 1)
                .Select(i => new string((Char)i, 1))
                .Concat(new String[1] {"0-9"} )
                .ToArray();
        }

        private static string GetAddress(String value)
        {
            return //value == "K" ? "/html[1]/body[1]/div[3]/div[3]/div[4]/table[1]/tr/td[1]/i/a" :
                value ==      "P" ? "/html[1]/body[1]/div[3]/div[3]/div[4]/div[3]/table[1]/tr/td[1]/i/a" :
                    value ==  "S" ? "/html[1]/body[1]/div[3]/div[3]/div[4]/table[2]/tr/td/i/a" :
                                    "/html[1]/body[1]/div[3]/div[3]/div[4]/div[1]/table[1]/tr/td[1]/i/a";
        }

        private static string[] PrepareTestData(string Filename)
        {
            if (File.Exists(Filename))
            {
                return File.ReadAllLines(Filename);
            }
            else
            {
                Console.WriteLine("Preparing test data - reading...");
                List<String> rs = new List<string>();
                foreach (var i in GetAlphabet())
                {
                    Console.Write(i);
                    string addr = String.Format("http://en.wikipedia.org/wiki/Index_of_Windows_games_({0})", i);
                    HtmlAgilityPack.HtmlWeb w = new HtmlAgilityPack.HtmlWeb();
                    HtmlDocument d = w.Load(addr);
                    try
                    {
                        rs.AddRange(d.DocumentNode.SelectNodes(GetAddress(i)).Select(t => t.InnerText));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine();
                        Console.WriteLine(string.Format("Error at {0}: {1}", i, e.Message));
                        throw;
                    }
                }

                File.WriteAllLines(Filename, rs);
                Console.WriteLine("Done!");

                return rs.ToArray();
            }
        }

        static void QueryOverStrings(string Path)
        {
            string Filename = Path + "\\games.txt";
            string[] currentGames = PrepareTestData(Filename);

            var output = from g in currentGames
                                         where g.Contains("gad")
                                         orderby g
                                         select HttpUtility.HtmlDecode(g);
            foreach (var i in output)
                Console.WriteLine(i);

            ReflectOverQueryResults(output);
        }

        static void ReflectOverQueryResults(Object resultSet)
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("Resultset type: {0}", resultSet.GetType().Name);
            Console.WriteLine("Resultset location: {0}", resultSet.GetType().Assembly.GetName().Name);

            Console.WriteLine("---------------------");
        }

        static void Main(string[] args)
        {
            QueryOverStrings(AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
