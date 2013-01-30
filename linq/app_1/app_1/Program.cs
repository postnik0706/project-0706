using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;

namespace app_1
{
    /*class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }*/
    
    class Program
    {
        private static void TestStandardLambda(Action<string> Action)
        {
            Action("test");
        }
        
        private static XDocument SampleProcesses()
        {
            return new XDocument(
                new XElement("Processes",
                    from v in Process.GetProcesses()
                    where v.ProcessName == "devenv"
                    select new XElement("Process",
                        v.ProcessName,
                        "test",
                        new XElement("Modules",
                            from m in v.Modules.Cast<ProcessModule>()
                            select new XElement("Module", m.FileName))))

                            );
        }

        /*private static Employee[] EmployeesSample()
        {
            return new Employee[] { 
                new Employee() { Id = 1, Name = "Alex" },
                new Employee() { Id = 2, Name = "Rob" },
                new Employee() { Id = 3, Name = "Anny" }
            };
        }
        */

        static void Main(string[] args)
        {
            NorthwindEntities context = new NorthwindEntities();

            IEnumerable<Order> orders = from o in context.Orders
                                        where o.ShippedDate.Value.Month == 5
                                        orderby o.OrderID descending
                                        select o;

            XElement doc = new XElement("Orders",
                new XElement("Order",
                    from v in orders
                    select new XElement("Order",
                        new XAttribute("OrderID", v.OrderID),
                        new XAttribute("OrderDate", v.OrderDate)
                        
                        )));

            Console.WriteLine(doc.ToString());

        }
    }
}