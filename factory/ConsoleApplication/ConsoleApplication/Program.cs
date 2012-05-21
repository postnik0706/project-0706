﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace ConsoleApplication
{
    [DataContract]
    public class SensorData
    {
        [DataMember]
        public int SensorID;
        
        [DataMember]
        public string SensorType;

        [DataMember]
        public bool IsAlarmed;
    }

    [DataContract]
    public class BuildingSensorsResponse
	{
        [DataMember]
        public List<SensorData> BuildingSensorData;
	}

    [ServiceContract(Namespace = "http://secured.fabric.com/2012/05/Security")]
    public interface ISecurityConsole
	{
        [OperationContract(Action="")]
        void ReportStatus(BuildingSensorsResponse StatusToReport, string Test);
	}

    public class SecurityConsole : ISecurityConsole
    {
        public void ReportStatus(BuildingSensorsResponse StatusToReport, string Test)
        {
            foreach (var item in StatusToReport.BuildingSensorData)
            {
                if (item.IsAlarmed)
                
                Console.WriteLine(item.SensorID);
            }
            Console.WriteLine("Test output");
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(SecurityConsole)))
            {
                Utilities.ShowInColor(String.Format("Console has started up: {0}", DateTime.UtcNow), ConsoleColor.Green);
                Utilities.ShowInColor("Press \"S\" to stop");

                Trace.TraceInformation("Starting");
                Trace.WriteLine("Starting", "Information");
                host.Open();
                
                while (Console.ReadKey(true).Key != System.ConsoleKey.S) ;
                
                host.Close();
            }
        }
    }
}