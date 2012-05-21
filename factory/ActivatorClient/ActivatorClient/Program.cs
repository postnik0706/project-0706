using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ActivatorClient
{
    public class CmdLine
    {
        private static CmdLine instance = null;
        
        private string buildingID = null;
        private bool? isToActivate = null;
        private bool helpRequired = false;
        
        public static CmdLine Parameters
        {
            get
            {
                if (instance == null)
                    instance = new CmdLine();
                return instance;
            }
        }

        public string BuildingID
        {
            get
            {
                if (buildingID == "")
                    throw new ArgumentException("BuildingID is not set");
                return buildingID;
            }
        }

        public bool HelpRequired
        {
            get
            {
                return helpRequired;
            }
        }

        public bool IsToActivate { 
            get 
            {
                if (!isToActivate.HasValue)
                    throw new ArgumentException("Activate/Deactivate is not set");
                return isToActivate.Value;
            }
        }

        public static void Load(string[] Params)
        {
            try
            {
                int i = 0;
                while (i < Params.Length)
                {
                    if (Params[i] == @"/?")
                        Parameters.helpRequired = true;
                    else if (Params[i].ToUpper() == @"/BLDG")
                    {
                        Parameters.buildingID = Params[i + 1];
                        i++;
                    }
                    else if (Params[i].ToUpper() == @"/ACTIVATE")
                        Parameters.isToActivate = true;
                    else if (Params[i].ToUpper() == @"/DEACTIVATE")
                        Parameters.isToActivate = false;
                    else
                        throw new ArgumentException("Unknown parameter");
                    i++;
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("Parameter value is incorrect");
            }

            if ((!Parameters.helpRequired) &&
                 ((Parameters.buildingID == "") || (!Parameters.isToActivate.HasValue)))
                throw new ArgumentException("Not all the parameters were set up");
        }

        public static void Reset()
        {
            instance = null;
        }
    }

    class Program
    {
        static void Help()
        {
            Utilities.ShowInColor(
@"Usage parameters:
/Bldg <Bldg ID> /Activate
/Bldg <Bldg ID> /Deactivate
", ConsoleColor.Green);
        }

        static int Main(string[] args)
        {
            try 
	        {
                CmdLine.Load(args);
	        }
	        catch (ArgumentException e)
	        {
                Utilities.ShowInColor(e.Message, ConsoleColor.Red);
                Help();
                return 1;
	        }

            if (CmdLine.Parameters.HelpRequired)
            {
                Help();
                return 1;
            }

            Buildings.Load();

            byte[] plaintext = Encoding.UTF8.GetBytes(
                String.Format("POLLING={0}\nACTIVATE={1}\nSECRET={2}",
                Buildings.Settings[CmdLine.Parameters.BuildingID].PollingInterval,
                CmdLine.Parameters.IsToActivate ? 1 : 0,
                Buildings.Settings[CmdLine.Parameters.BuildingID].Secret));

            WebRequest request = WebRequest.Create(
                Buildings.Settings[CmdLine.Parameters.BuildingID].URI);
            
            Console.WriteLine("Requesting {0}", Buildings.Settings[CmdLine.Parameters.BuildingID].URI);
            Utilities.ShowInColor(
                String.Format("  for {0}", CmdLine.Parameters.IsToActivate ? "activation": "deactivation"),
                CmdLine.Parameters.IsToActivate ? ConsoleColor.Green: ConsoleColor.Red);

            request.Method = "POST";
            request.ContentLength = plaintext.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(plaintext, 0, plaintext.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine("Response: {0}", ((HttpWebResponse)response).StatusDescription);

            return 0;
        }
    }
}