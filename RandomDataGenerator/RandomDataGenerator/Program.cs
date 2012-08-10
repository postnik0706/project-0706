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
        private const string CACHE_FILENAME_FIRST_NAMES_FEMALE = @"./dist.female.first";
        private const string CACHE_FILENAME_FIRST_NAMES_MALE = @"./dist.male.first";

        static void Main(string[] args)
        {
            // Census - most popular US names
            WebClient census = new WebClient();
            census.DownloadFile("http://www.census.gov/genealogy/www/data/1990surnames/dist.female.first", CACHE_FILENAME_FIRST_NAMES_FEMALE);
            census.DownloadFile("http://www.census.gov/genealogy/www/data/1990surnames/dist.female.first", CACHE_FILENAME_FIRST_NAMES_MALE);
            census.DownloadFile("http://www.census.gov/genealogy/www/data/1990surnames/dist.all.last", CACHE_FILENAME_LAST_NAMES);

            string[] lastNames = (from n in File.ReadAllLines(CACHE_FILENAME_LAST_NAMES)
                              select n.Substring(0, 1).ToUpper() + (n.Substring(1, n.IndexOf(" ") - 1)).ToLower())
                              .ToArray();
            Console.WriteLine("will process: LastNames {0}", lastNames.Count());
            string[] firstNames = (from n in File.ReadAllLines(CACHE_FILENAME_FIRST_NAMES_FEMALE)
                                   select n.Substring(0, 1).ToUpper() + (n.Substring(1, n.IndexOf(" ") - 1)).ToLower())
                                   .ToArray();
            Console.WriteLine("will process: FirstNames {0}", firstNames.Count());
            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            var names = from ln in lastNames
                             join fn in firstNames on 1 equals 1
                             select fn + " " + ln;

            foreach (var name in names)
            {
                Console.WriteLine(name);
            }
        }
    }
}