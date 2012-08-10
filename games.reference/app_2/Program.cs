using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace app_2
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public override string ToString()
        {
            return String.Format("----------------------\n  First Name: {0}\n  Last Name: {1}\n  Date of birth: {2}",
                FirstName, LastName, DateOfBirth);
        }
    }

    public static class StringEnh
    {
        public static string GetSHA1Hash(this string text)
        {
            SHA256Managed s = new SHA256Managed();
            return Convert.ToBase64String(
                s.ComputeHash(new UnicodeEncoding().GetBytes(text)));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string s = "test";
            //Console.WriteLine(s.CreateHyperlink("http://www.google.com"));
            Console.WriteLine(s.GetSHA1Hash());
            Console.WriteLine("This is a test".GetSHA1Hash());

            var c = new Contact()
            {
                LastName = "Test",
                FirstName = "Alex",
                DateOfBirth=new DateTime(2012, 3, 3)
            };

            var list = new List<Contact>()
            {
                new Contact() { FirstName = "Another",
                    LastName = "Contact",
                    DateOfBirth = new DateTime(2011, 2, 3)}
            };

            Console.WriteLine(c);

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

            var list1 = new[] {
                new { LastName = "Alex", Salary = 5.55}
            };

            Console.WriteLine("----------------------");
            foreach (var item in list1)
            {
                Console.WriteLine(item);
            }

            Array a = new[] {
                1, 2, 3, 4, 5, 6, 7
            };
            a.ForEach
        }
    }
}
