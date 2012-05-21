using System;
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
        public string BuildingID;

        [DataMember]
        public int SensorID;
        
        [DataMember]
        public string SensorType;

        [DataMember]
        public bool IsAlarmed;

        [DataMember]
        public DateTime MeasureDateTime;
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
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("------------------------------------------------------------------");
            TraceSource ts = new TraceSource("consoleLog");


            bool isBuildingOk = true;
            foreach (var item in StatusToReport.BuildingSensorData)
            {
                if (item.IsAlarmed)
                {
                    isBuildingOk = false;
                    break;
                }
            }

            if (isBuildingOk)
            {
                string msg = String.Format("Building {0} is OK at: {1}", StatusToReport.BuildingSensorData[0].BuildingID,
                    StatusToReport.BuildingSensorData[0].MeasureDateTime);
                ts.TraceEvent(TraceEventType.Information, 0, msg);
                Console.WriteLine(msg);
            }
            else
            {

                foreach (var item in StatusToReport.BuildingSensorData)
                {
                    ConsoleColor? color = null;
                    if (item.IsAlarmed)
                    {
                        isBuildingOk = false;
                        color = ConsoleColor.Red;
                    }

                    string msg = String.Format("  Building: {0}", item.BuildingID);
                    ts.TraceEvent(item.IsAlarmed ? TraceEventType.Warning : TraceEventType.Information, 0, msg);
                    Utilities.ShowInColor(msg, color);
                    msg = String.Format("  Sensor  : {0}", item.SensorID);
                    ts.TraceEvent(item.IsAlarmed ? TraceEventType.Warning : TraceEventType.Information, 0, msg);
                    Utilities.ShowInColor(msg, color);
                    msg = String.Format("  Type    : {0}", item.SensorType);
                    ts.TraceEvent(item.IsAlarmed ? TraceEventType.Warning : TraceEventType.Information, 0, msg);
                    Utilities.ShowInColor(msg, color);
                    if (item.IsAlarmed)
                    {
                        msg = "  Status  : Is Alarmed";
                        ts.TraceEvent(item.IsAlarmed ? TraceEventType.Warning : TraceEventType.Information, 0, msg);
                        Utilities.ShowInColor(msg, color);
                    }
                    else
                    {
                        msg = "  Status  : Is not alarmed";
                        ts.TraceEvent(item.IsAlarmed ? TraceEventType.Warning : TraceEventType.Information, 0, msg);
                        Utilities.ShowInColor("  Status  : Is not alarmed");
                    }
                    Utilities.ShowInColor(String.Format("  Time    : {0}", item.MeasureDateTime), color);
                    Console.WriteLine();
                }
            }
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(SecurityConsole)))
            {
                Utilities.ShowInColor(String.Format("Console has started up: {0}", DateTime.Now), ConsoleColor.Green);
                Utilities.ShowInColor("Press \"S\" to stop");

                Trace.WriteLine("Starting", "Information");
                host.Open();
                
                while (Console.ReadKey(true).Key != System.ConsoleKey.S) ;
                
                host.Close();
            }
        }
    }
}