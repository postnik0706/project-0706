using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.Reflection;

namespace plural.practice
{
    public delegate int test(int x, int y);

    public class Program
    {
        private static void QueryEmployees()
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee() { ID=1, Name="Scott", HireDate=new DateTime(2002, 3, 5) },
                new Employee() { ID=2, Name="Alex", HireDate=new DateTime(2003, 1, 5) },
                new Employee() { ID=3, Name="Raya", HireDate=new DateTime(2001, 12, 25) },
            };

            IEnumerable<Employee> query =
                from e in employees
                where e.HireDate.Year <= 2002
                orderby e.Name
                select e;

            employees.Add(new Employee() { ID = 4, Name = "Linda", HireDate = new DateTime(2001, 12, 1) });

            foreach (Employee e in query)
            {
                Console.WriteLine(e.Name);
            }
        }

        private static void CreateSimpleXml()
        {
            XNamespace ns = "http://www.tenzee.com";
            XNamespace ext = "http://www.google.com/Modules/Ext";

            /*XDocument doc = new XDocument(
                new XElement(ns + "Modules",
                    new XAttribute(XNamespace.Xmlns + "ext", ext),
                    new XElement(ns + "Module", "Introduction to LINQ"),
                    new XElement(ns + "Module", "LINQ and C#"),
                    new XElement(ext + "Extra", "Some Content"),
                    new XElement(ext + "Extra", "Another")
                    ));
            doc.Save("modules.xml");
            */

            XDocument doc = new XDocument(
                new XElement(ns + "Processes",
                    from p in Process.GetProcesses()
                    where p.ProcessName == "devenv"
                    select new XElement(ns + "Process",
                        new XAttribute("Name", p.ProcessName),
                        new XElement(ns + "Modules",
                            from m in p.Modules.Cast<ProcessModule>()
                            select new XElement(ns + "Module", m.FileName))
                        )));
        }

        private static void ReadSimpleXml()
        {
            XNamespace ns = "http://www.tenzee.com";
            XDocument doc = XDocument.Load("modules.xml");

            var elements =
                doc.Descendants(ns + "Module");

            foreach (var element in elements)
            {
                string value = (string)element;
            }
        }

        private static void CreateTypeInfo()
        {
            XDocument doc = new XDocument(
                new XElement("Types",
                    Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                            .Select(name => Assembly.Load(name))
                            .Select(assembly => assembly.GetTypes())));
        }

        private static void QueryTypes()
        {
            IEnumerable<string> publicTypes =
                from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsPublic
                select t.FullName;

            foreach (string name in publicTypes)
            {
                Console.WriteLine(name);
            }
        }

        private static void QueryXml()
        {
            XDocument doc = new XDocument(
                new XElement("Processes",
                    from p in Process.GetProcesses()
                    orderby p.ProcessName ascending
                    select new XElement("Process",
                        new XAttribute("Name", p.ProcessName),
                        new XAttribute("PID", p.Id))));

            IEnumerable<int> pids =
                from e in doc.Element("Processes").Elements("Process")
                where e.Attribute("Name").Value == "devenv"
                orderby (int)e.Attribute("PID") ascending
                select (int)e.Attribute("PID");

            foreach (int id in pids)
            {
                Console.WriteLine(id);
            }
        }

        private static void ShowDbProviders()
        {
            foreach (System.Data.DataRow i in System.Data.Common.DbProviderFactories.GetFactoryClasses().Rows)
            {
                Console.WriteLine("---------------- Row");
                foreach (System.Data.DataColumn j in i.Table.Columns)
                {
                    Console.WriteLine(String.Format("Name {0} = {1}", j.ColumnName, i[j]));
                }
            }
        }

        static void Main(string[] args)
        {
            //CreateSimpleXml();
            //ReadSimpleXml();
            //CreateTypeInfo();
            //QueryEmployees();
            //QueryTypes();
            //QueryXml();

            //ShowDbProviders();

            MovieReviews db = new MovieReviews();
            db.Database.CreateIfNotExists();
            Console.WriteLine("** DONE");

            Console.ReadLine();
        }
    }
}
