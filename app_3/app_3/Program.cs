using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace app_3
{
    static class StringUtils
    {
        public static string AsUrl(this string Input, string URL)
        {
            return String.Format(@"<a href ""{0}"">{1}</a>", URL, Input);
        }
    }

    class Program
    {
        delegate void Test(Object Sender, int i);
        static event Test MyEvent;

        static void MyHandler(Object Sender, int i)
        {
            Console.WriteLine("This is my handler {0}", i);
        }

        static void InvokeDelegate(Test Event, int input)
        {
            Event(null, input);
        }

        static void Main(string[] args)
        {
            int[] a = { 0, 1, 2, 3, 4, 5, 6, 7 };
            var rs = from n in a
                     where n <= 5
                     orderby n descending
                     select n;

            foreach (var i in rs)
            {
                Console.WriteLine(i);
            }

            Test t = new Test(MyHandler);
            t.Invoke(null, 123);

            MyEvent += new Test(Program_MyEvent);
            MyEvent.Invoke(null, 1234);
        }

        static void Program_MyEvent(object Sender, int i)
        {
            Console.WriteLine("This is my event: {0}", i);

            List<int> t = new List<int>() { 0, 1, 2, 3, 4, 5 };
            foreach (var item in t)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("This is a test".AsUrl("www.google.com"));

            var cars = new[] { new { Name = "Car", Price = 1000.00 },
                new { Name = "Book", Price = 199.00 } };
            Console.WriteLine(cars);

            foreach (var item in cars)
            {
                Console.WriteLine("Name: {0}, Price: {1}", item.Name, item.Price);
            }

            InvokeDelegate(delegate(Object obj, int t1) { Console.WriteLine("t1 is {0}", t1); }, 2000);
            InvokeDelegate(
                (Object obj, int t2) => { Console.WriteLine("t2 is {0}", t2); }, 3123);


            foreach (var item in cars.Where(s => s.Name == "Car"))
            {
                Console.WriteLine(item.Name);
            };
        }
    }
}
