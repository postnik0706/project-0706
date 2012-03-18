using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace test_app3
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ReadAndGenerateTestClasses()
        {
            File.WriteAllText("out.txt", "This is a test");

            Console.WriteLine(Assembly.GetCallingAssembly().Location);
            //string[] lines = File.ReadAllLines(@"..\Input.txt");
            //Console.WriteLine(lines.Count());
        }
    }
}
