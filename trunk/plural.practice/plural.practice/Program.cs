using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace plural.practice
{
    class Program
    {
        static void Main(string[] args)
        {
            XDocument doc = new XDocument(
                new XElement("Modules",
                    new XElement("Module", "Introduction to LINQ"),
                    new XElement("Module", "LINQ and C#")));
            doc.Save("modules.xml");
        }
    }
}
