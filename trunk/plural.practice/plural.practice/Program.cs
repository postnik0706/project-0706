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
            XNamespace ns = "http://www.tenzee.com";
            XNamespace ext = "http://www.tenzee.com/Modules/Ext";
            
            XDocument doc = new XDocument(
                new XElement(ns + "Modules",
                    new XAttribute(XNamespace.Xmlns + "ext", ext),
                    new XElement(ns + "Module", "Introduction to LINQ"),
                    new XElement(ns + "Module", "LINQ and C#"),
                    new XElement(ext + "Extra", "Some Content"),
                    new XElement(ext + "Extra", "Another")
                    
                    ));
            doc.Save("modules.xml");
        }
    }
}
