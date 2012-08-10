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
            XNamespace ns = "http://www.tenzee.com";
            XDocument doc = XDocument.Load("modules.xml");

            var elements = 
                doc.Descendants(ns + "Module");

            foreach (var element in elements)
	        {
                string value = (string)element;
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
