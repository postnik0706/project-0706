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
            CreateSimpleXml();
            ReadSimpleXml();
        }

        private static void ReadSimpleXml()
        {
            XDocument doc = XDocument.Load("modules.xml");

            XElement root = doc.Root;

            var elements = root.Descendants();

            foreach (var element in elements)
            {
                string value = element.Value;

                string length = (string)element.Attribute("Length");
            }
        }

        private static void CreateSimpleXml()
        {
            XDocument doc = new XDocument(
                new XElement("Modules",
                    new XElement("Module", "Introduction to LINQ"),
                    new XElement("Module", "LINQ and C#")));
            doc.Save("modules.xml");
        }
    }
}
